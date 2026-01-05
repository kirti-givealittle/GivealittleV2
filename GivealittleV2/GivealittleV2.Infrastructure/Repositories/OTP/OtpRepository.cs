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
        private readonly IEmailService _sender;

        public OtpRepository(ApplicationDbContext db, IOptions<OtpOptions> options, IEmailService sender)
        {
            _db = db;
            _options = options.Value;
            _sender = sender;
        }

        public async Task<OtpCreateResult> CreateAndSendOTPAsync(string otp,string userEmail,string userFName, OtpPurpose purpose)
        {
            var now = DateTime.UtcNow;

            // One active OTP per user+purpose; enforce resend cooldown
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

            // Send via your email client (HTML template etc.)
            var subject = "Your verification code";

            await _sender.SendAsync(
                to: userEmail,
                subject: subject,
                body: await _sender.RenderOTPTemplate(userFName, otp, _options.ExpiryMinutes),
                isHtml: true);

            return new OtpCreateResult(record.Id, record.ExpiresAtUtc, record.NextResendAllowedAtUtc);
        }

        /// <summary>
        /// Returns the most recent active OTP for user + purpose
        /// </summary>
        private async Task<OtpRecord?> GetActiveAsync(
            string userKey,
            OtpPurpose purpose)
        {
            var now = DateTime.UtcNow;

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

        public async Task<OtpValidationResult> ValidateOTPAsync(string userKey, OtpPurpose purpose, string otp)
        {
            var now = DateTime.UtcNow;

            var record = await GetActiveAsync(userKey, purpose);
            if (record is null)
                return new OtpValidationResult(false, "No active OTP found.");

            if (record.UsedAtUtc is not null)
            {
                await IncrementFailedAttemptAsync(record);
                return new OtpValidationResult(false, "OTP already used.");
            }
                

            if (record.ExpiresAtUtc <= now)
            {
                await IncrementFailedAttemptAsync(record);
                return new OtpValidationResult(false, "OTP expired.");
            }
                

            if (record.LockedUntilUtc.HasValue && record.LockedUntilUtc.Value > now)
                return new OtpValidationResult(false, "Too many attempts. Try again later.");

            // Hash the provided OTP using stored salt + pepper and compare
            var attemptedHash = OtpHashing.HashOtpWithSalt(otp, _options.Pepper, record.Salt);

            bool match = OtpHashing.FixedTimeEquals(attemptedHash, record.OtpHash);

            if (!match)
            {
                await IncrementFailedAttemptAsync(record);

                return new OtpValidationResult(false, "Invalid OTP.");
            }

            await MarkUsedAsync(record);
            return new OtpValidationResult(true);
        }
    }
}
