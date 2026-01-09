using GivealittleV2.Application.Interfaces.Email;
using GivealittleV2.Application.Interfaces.OTP;
using GivealittleV2.Domain.Models.OTP;
using GivealittleV2.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GivealittleV2.Infrastructure.Repositories.OTP
{
    public sealed class OtpRepository : IOtpRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly OtpOptions _options;
        private readonly IEmailSender _sender;
        private readonly IEmailDtoProvider emailDtoProvider;

        public OtpRepository(ApplicationDbContext db, IOptions<OtpOptions> options, IEmailSender sender, IEmailDtoProvider _emailDtoProvider)
        {
            _db = db;
            _options = options.Value;
            _sender = sender;
            emailDtoProvider = _emailDtoProvider;
        }

        public async Task<OtpCreateResult> CreateAndSendOTPAsync(string otp,string userEmail,string userFName, OtpPurpose purpose)
        {
            var now = DateTime.UtcNow;

            var existing = await GetActiveAsync(userEmail, purpose);
            if (existing is not null)
            {
                if (existing.UsedAtUtc is null && existing.ExpiresAtUtc > now)
                {
                    if (existing.NextResendAllowedAtUtc.HasValue && existing.NextResendAllowedAtUtc.Value > now)
                    {
                        return new OtpCreateResult(existing.Id, existing.ExpiresAtUtc, existing.NextResendAllowedAtUtc);
                    }
                }
            }


            var (hash, salt) = OtpHashing.HashOtp(otp, _options.Pepper);

            var record = new OtpRecord
            {
                UserEmail = userEmail,
                Purpose = (int)purpose,
                OtpHash = hash,
                Salt = salt,
                CreatedAtUtc = now,
                ExpiresAtUtc = now.AddMinutes(_options.ExpiryMinutes),
                FailedAttempts = 0,
                MaxAttempts = _options.MaxAttempts,
                LockedUntilUtc = null,
                UsedAtUtc = null,
                NextResendAllowedAtUtc = now.AddSeconds(_options.ResendCooldownSeconds)
            };

            await UpsertAsync(record);

            var req = new EmailSendRequestDTO(
                userEmail,
                userFName,
                GetEmailTemplateKey(purpose), 
                emailDtoProvider.GetEmailDto(new OtpEmailDto(
                    otp, 
                    userFName, 
                    purpose.ToString(), 
                    _options.ExpiryMinutes, 
                    _db.GlobalConfigs.Where(x => x.Key == "SYSTEM_EMAIL").Select(x => x.Value).FirstOrDefault() ?? "",
                    purpose == OtpPurpose.Registration ? record.Id.ToString() : userEmail
                    )));
            await _sender.SendAsync(req);

            return new OtpCreateResult(record.Id, record.ExpiresAtUtc, record.NextResendAllowedAtUtc);
        }

        private string GetEmailTemplateKey(OtpPurpose purpose)
        {
            switch (purpose)
            {
                case OtpPurpose.Login:
                    return "OTP_VERIFICATION";
                case OtpPurpose.Registration:
                    return "OTP_ACCOUNT_REGISTRATION_WITH_VERIFICATION_LINK";
                case OtpPurpose.PasswordReset:
                    return "OTP_PASSWORD_RESET";
                default: throw new ArgumentOutOfRangeException(nameof(purpose), purpose, "Unsupported OTP purpose.");
            }
        }

        /// <summary>
        /// Returns the most recent active OTP for user + purpose
        /// </summary>
        private async Task<OtpRecord?> GetActiveAsync(
            string userKey,
            OtpPurpose purpose)
        {
            var now = DateTime.UtcNow;
            if(purpose == OtpPurpose.Registration)
            {
                userKey = _db.OtpRecords.Where(x => x.Id.ToString() == userKey).Select(x => x.UserEmail).FirstOrDefault() ?? "";
            }
            var otpObj = await _db.OtpRecords
                .Where(x =>
                    x.UserEmail == userKey &&
                    x.Purpose == (int)purpose &&
                    x.UsedAtUtc == null &&
                    x.ExpiresAtUtc > now)
                .OrderByDescending(x => x.CreatedAtUtc)
                .FirstOrDefaultAsync();

            if (otpObj is null) return null;

            return otpObj;
        }

        private async Task UpsertAsync(OtpRecord record)
        {
            await _db.OtpRecords.AddAsync(record);

            await _db.SaveChangesAsync();
        }

        private async Task MarkUsedAsync(
            OtpRecord record)
        {
            record.UsedAtUtc = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }

        private async Task IncrementFailedAttemptAsync(
            OtpRecord otp)
        {
            otp.FailedAttempts++;

            if (otp.FailedAttempts >= otp.MaxAttempts)
            {
                otp.LockedUntilUtc = DateTime.UtcNow.AddMinutes(_options.LockMinutes);
            }

            await _db.SaveChangesAsync();
        }

        public async Task<OtpValidationResult> ValidateOTPAsync(string userEmail, OtpPurpose purpose, string otp)
        {
            var now = DateTime.UtcNow;

            var record = await GetActiveAsync(userEmail, purpose);
            if (record is null)
            {
                return new OtpValidationResult(false, "No active OTP found.", _db.EmailTemplates
                    .Where(x => x.TemplateKey == "OTP_VERIFICATION_FAILED")
                    .Select(x => x.BodyTemplate)
                    .FirstOrDefault());
            }

                

            if (record.ExpiresAtUtc <= now)
            {
                await IncrementFailedAttemptAsync(record);
                return new OtpValidationResult(false, "OTP expired.", _db.EmailTemplates
                    .Where(x => x.TemplateKey == "OTP_VERIFICATION_FAILED")
                    .Select(x => x.BodyTemplate)
                    .FirstOrDefault());
            }
                

            if (record.LockedUntilUtc.HasValue && record.LockedUntilUtc.Value > now)
                return new OtpValidationResult(false, "Too many attempts. Try again later.", _db.EmailTemplates
                    .Where(x => x.TemplateKey == "OTP_VERIFICATION_FAILED")
                    .Select(x => x.BodyTemplate)
                    .FirstOrDefault());

            // Hash the provided OTP using stored salt + pepper and compare
            var attemptedHash = OtpHashing.HashOtpWithSalt(otp, _options.Pepper, record.Salt);

            bool match = OtpHashing.FixedTimeEquals(attemptedHash, record.OtpHash);

            if (!match)
            {
                await IncrementFailedAttemptAsync(record);

                return new OtpValidationResult(false, "Invalid OTP.", _db.EmailTemplates
                    .Where(x => x.TemplateKey == "OTP_VERIFICATION_FAILED")
                    .Select(x => x.BodyTemplate)
                    .FirstOrDefault());
            }

            await MarkUsedAsync(record);
            var usermail = _db.EntityEmails.Where(x => x.EmailAddress == userEmail).FirstOrDefault();
            if (usermail != null)
            {
                usermail.IsVerified = true;
                await _db.SaveChangesAsync();
            }

            RemoveOtpRecord(record);
            // With this line:
            return new OtpValidationResult(
                true,
                null,
                _db.EmailTemplates
                    .Where(x => x.TemplateKey == "OTP_VERIFIED")
                    .Select(x => x.BodyTemplate)
                    .FirstOrDefault()
            );
        }

        private void RemoveOtpRecord(OtpRecord record)
        {
             _db.OtpRecords.Remove(record);
            _db.SaveChanges();
        }
    }
}
