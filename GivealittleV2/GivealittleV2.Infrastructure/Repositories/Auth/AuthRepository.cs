using GivealittleV2.Application.Interfaces.Auth;
using GivealittleV2.Application.Interfaces.Email;
using GivealittleV2.Application.Interfaces.OTP;
using GivealittleV2.Domain.Models.Auth.DTOs;
using GivealittleV2.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;

namespace GivealittleV2.Infrastructure.Repositories.Auth
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _config;
        private readonly ITokenService _tokens;
        private readonly IOtpService otpService;

        public AuthRepository(ApplicationDbContext dbContext, IConfiguration configuration, ITokenService tokens, IOtpService _otpService)
        {
            _db = dbContext;
            _config = configuration;
            _tokens = tokens;
            otpService = _otpService;
        }

        public async Task<AuthResponseDTO> RegisterUserAsync(RegistrationDTO req, string? ip, string? ua)
        {
            var email = req.Email.Trim();
            var normalized = email.ToUpperInvariant();

            var existingUser = CheckExistingUser(req);

            if (existingUser != null) { return existingUser; };

            var entity = new Persistence.Models.Entity
            {
                IsIndividual = true,
                EntityEmails = new List<EntityEmail>
                {
                    new EntityEmail
                    {
                        EmailAddress = email,
                        IsDefault = true
                    }
                },
                Individual = new Individual
                {
                    FirstName = req.FName.Trim(),
                    LastName = req.LName.Trim(),
                    DateOfBirth = new DateTime(1900, 1, 1)
                }
            };

            _db.Entities.Add(entity);

            await _db.SaveChangesAsync();

            await otpService.GenerateAndSendTokenEmail(req.Email, req.FName, OtpPurpose.Registration);
            //await otpService.CreateAndSendAsync(req.Email, req.FName, OtpPurpose.Registration);

            return new AuthResponseDTO(
                UserId: entity.Id,
                AccessToken: "",
                AccessTokenExpiresAtUtc: DateTime.MinValue,
                RefreshToken: "",
                FName: req.FName,
                LName: req.LName,
                Emails: new List<string> { email },
                IsExistingUser: false
            );
        }

       
        public AuthResponseDTO? CheckExistingUser(RegistrationDTO req)
        {
            if (!string.IsNullOrEmpty(req.FName) &&
                !string.IsNullOrEmpty(req.LName) && !string.IsNullOrEmpty(req.Email))
            {
                var existing = _db.Individuals
                    .Where(i =>
                     i.IdNavigation != null &&
                     i.IdNavigation.EntityEmails
                        .Any(em => em.EmailAddress.ToLower() == req.Email.Trim().ToLower()) == true &&
                    i.FirstName == req.FName.Trim() &&
                    i.LastName == req.LName.Trim())
                    .FirstOrDefault();


                if (existing != null)
                {
                    return new AuthResponseDTO(
                        UserId: existing.Id,
                        AccessToken: "",
                        AccessTokenExpiresAtUtc: DateTime.MinValue,
                        RefreshToken: "",
                        FName: existing.FirstName,
                        LName: existing.LastName,
                        Emails: existing.IdNavigation?.EntityEmails
                            .Select(em => em.EmailAddress)
                            .ToList() ?? new List<string>(),
                        IsExistingUser: true
                    );
                }

                return null;
            }
            return null;
        }

        public async Task<AuthResponseDTO> LoginUserAsync(LoginRequestDTO req, string? ip, string? ua)
        {
            var user = _db.Individuals
                    .Where(i =>
                     i.IdNavigation != null &&
                     i.IdNavigation.EntityEmails
                        .Any(em => em.EmailAddress.ToLower() == req.Email.Trim().ToLower()) == true)
                    .FirstOrDefault();

            if (user == null)
            {
                await AddAuditAsync(null, req.Email, false, "LOGIN", "USER_NOT_FOUND", ip, ua);
                throw new InvalidOperationException("Invalid credentials.");
            }

            await otpService.CreateAndSendAsync(req.Email, user.FirstName, OtpPurpose.Login);

            return new AuthResponseDTO(
                UserId: user.Id,
                AccessToken: "",
                AccessTokenExpiresAtUtc: DateTime.MinValue,
                RefreshToken: "",
                FName: user.FirstName,
                LName: user.LastName,
                Emails: user.IdNavigation?.EntityEmails
                    .Select(em => em.EmailAddress)
                    .ToList() ?? new List<string>(),
                IsExistingUser: true
            );

            

            
        }

        public async Task<AuthResponseDTO> LoginVerifyAsync(LoginVerifyOtpDTO req, string? ip, string? ua)
        {
            var user = _db.Individuals
            .Where(i =>
                i.IdNavigation != null &&
                i.IdNavigation.EntityEmails
                    .Any(em => em.EmailAddress.ToLower() == req.userEmail.Trim().ToLower()) == true)
                    .FirstOrDefault();
            if (user == null)
            {
                await AddAuditAsync(null, req.userEmail, false, "LOGIN", "USER_NOT_FOUND", ip, ua);
                throw new InvalidOperationException("Invalid credentials.");
            }

            var otpVerificationResult = await otpService.ValidateAsync(req.userEmail, OtpPurpose.Login, req.OTP);
            if (!otpVerificationResult.IsValid)
            {
                await AddAuditAsync(user.Id, req.userEmail, false, "LOGIN", "INVALID_OTP", ip, ua);
                throw new InvalidOperationException("Invalid OTP.");
            }
            return await GrantLogin(user, req.userEmail, ip, ua);
        }

        private async Task<AuthResponseDTO> GrantLogin(Individual user, string email, string ip, string ua)
        {
            await AddAuditAsync(user.Id, email, true, "LOGIN", null, ip, ua);
            await _db.SaveChangesAsync();

            return await IssueTokensAsync(user, email, ip, ua);
        }

        private Task AddAuditAsync(Guid? authUserId, string? email, bool success, string eventType, string? reason, string? ip, string? ua)
        {
            _db.AuthLoginAudits.Add(new AuthLoginAudit
            {
                OccurredAtUtc = DateTime.UtcNow,
                AuthUserId = authUserId,
                Email = email,
                Success = success,
                EventType = eventType,
                FailureReason = reason,
                IpAddress = ip,
                UserAgent = ua
            });

            return Task.CompletedTask;
        }

        private async Task<AuthResponseDTO> IssueTokensAsync(Individual user, string email, string? ip, string? ua)
        {
            //var roles = await GetRolesAsync(user.Id);

            var (accessToken, accessTokenExp) = _tokens.CreateAccessToken(user.Id, email, new List<string>() { "user"});

            var refreshDays = int.Parse(_config["Jwt:RefreshTokenDays"]!);
            var (raw, hash, refreshExp) = _tokens.CreateRefreshToken(refreshDays);


            var rt = new AuthRefreshToken
            {
                RefreshTokenId = Guid.NewGuid(),
                AuthUserId = _db.EntityEmails.Where(x => x.EmailAddress == email).Select(x => (Guid?)x.Id).FirstOrDefault() ?? Guid.Empty,
                TokenHash = hash,
                CreatedAtUtc = DateTime.UtcNow,
                ExpiresAtUtc = refreshExp,
                CreatedByIp = ip,
                UserAgent = ua
            };

            _db.AuthRefreshTokens.Add(rt);
            await _db.SaveChangesAsync();

            return new AuthResponseDTO(
                accessToken,
                accessTokenExp,
                raw,
                user.FirstName,
                user.LastName,
                _db.EntityEmails.Where(x => x.EmailAddress == email).Select(em => em.EmailAddress).ToList(),
                true,
                user.Id
            );
        }

        private async Task<List<string>> GetRolesAsync(Guid authUserId)
        {
            //return await _db.AuthUserRoles
            //    .Where(ur => ur.AuthUserId == authUserId)
            //    .Join(_db.AuthRoles, ur => ur.AuthRoleId, r => r.AuthRoleId, (_, r) => r.Name)
            //    .ToListAsync();

            throw new NotImplementedException();
        }

        public async Task<AuthResponseDTO> RefreshUserAsync(string refreshToken, string? ip, string? ua)
        {
            var tokenHash = _tokens.Sha256(refreshToken);

            var existing = await _db.AuthRefreshTokens
                .SingleOrDefaultAsync(t => t.TokenHash == tokenHash);

            if (existing == null || existing.RevokedAtUtc != null || existing.ExpiresAtUtc <= DateTime.UtcNow)
            {
                await AddAuditAsync(null, null, false, "REFRESH", "INVALID_REFRESH", ip, ua);
                throw new InvalidOperationException("Invalid refresh token.");
            }
            var user = await _db.EntityEmails.SingleOrDefaultAsync(x => x.Id == existing.AuthUserId);
            if (user == null)
            {
                await AddAuditAsync(null, "", false, "REFRESH", "USER_NOT_FOUND", ip, ua);
                throw new InvalidOperationException("User not found for refresh token.");
            }
            // Rotate refresh token
            var refreshDays = int.Parse(_config["Jwt:RefreshTokenDays"]!);
            var (newRaw, newHash, newExp) = _tokens.CreateRefreshToken(refreshDays);

            var newRt = new AuthRefreshToken
            {
                RefreshTokenId = Guid.NewGuid(),
                AuthUserId = user.Id,
                TokenHash = newHash,
                CreatedAtUtc = DateTime.UtcNow,
                ExpiresAtUtc = newExp,
                CreatedByIp = ip,
                UserAgent = ua
            };

            existing.RevokedAtUtc = DateTime.UtcNow;
            existing.ReplacedByRefreshTokenId = newRt.RefreshTokenId;

            _db.AuthRefreshTokens.Add(newRt);

            await AddAuditAsync(user.Id, user.EmailAddress, true, "REFRESH", null, ip, ua);
            await _db.SaveChangesAsync();

            var roles = await GetRolesAsync(user.Id);
            var (jwt, jwtExp) = _tokens.CreateAccessToken(user.Id, user.EmailAddress, roles);

            return new AuthResponseDTO(jwt, jwtExp, newRaw,"","",new List<string>(), true, user.Id);
        }

        public async Task LogoutUserAsync(string refreshToken, string? ip, string? ua)
        {
            var tokenHash = _tokens.Sha256(refreshToken);

            var existing = await _db.AuthRefreshTokens
                .SingleOrDefaultAsync(t => t.TokenHash == tokenHash);

            if (existing == null)
            {
                await AddAuditAsync(null, null, false, "LOGOUT", "INVALID_REFRESH", ip, ua);
                return;
            }

            existing.RevokedAtUtc = DateTime.UtcNow;
            await AddAuditAsync(existing.AuthUserId, null, true, "LOGOUT", null, ip, ua);

            await _db.SaveChangesAsync();
        }

    }
}
