using GivealittleV2.API.Auth.Interfaces;
using GivealittleV2.Domain.Models.Auth;
using GivealittleV2.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace GivealittleV2.API.Auth.Services
{
    public sealed class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _db;
        private readonly ITokenService _tokens;
        private readonly IConfiguration _config;

        public AuthService(ApplicationDbContext db, IPasswordService passwords, ITokenService tokens, IConfiguration config)
        {
            _db = db;
            _tokens = tokens;
            _config = config;
        }

        public async Task<AuthResponseDTO> RegisterAsync(RegistrationDTO req, string? ip, string? ua)
        {
            var email = req.Email.Trim();
            var normalized = email.ToUpperInvariant();

            var existingUser = CheckExistingUser(req);

            if (existingUser != null) { return existingUser; };

            var entity= new Entity
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
                    LastName = req.LName.Trim()
                }
            };

            _db.Entities.Add(entity);

            // Assign default role "User" (optional)


            await _db.SaveChangesAsync();
            //return await IssueTokensAsync(user, ip, ua);

            return new AuthResponseDTO(
                authUserId : entity.Id,
                AccessToken: "",
                AccessTokenExpiresAtUtc: DateTime.MinValue,
                RefreshToken: "",
                FName: req.FName,
                LName: req.LName,
                Emails: new List<string> { email },
                IsExistingUser: false
            );
        }

       

        private AuthResponseDTO? CheckExistingUser(RegistrationDTO req)
        {
            if (!string.IsNullOrEmpty(req.FName) &&
                !string.IsNullOrEmpty(req.LName) && !string.IsNullOrEmpty(req.Email))
            {
                var existing = _db.Individuals
                    .Where(i =>
                     (i.IdNavigation != null &&
                     i.IdNavigation.EntityEmails
                        .Any(em => em.EmailAddress.ToLower() == req.Email.Trim().ToLower()) == true) || 
                    (i.FirstName == req.FName.Trim() &&
                    i.LastName == req.LName.Trim()))
                    .FirstOrDefault();


                if (existing != null)
                {
                    return new AuthResponseDTO(
                        authUserId : existing.Id,
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

        public async Task<AuthResponseDTO> LoginAsync(LoginRequestDTO req, string? ip, string? ua)
        {
            var email = req.Email.Trim();
            var normalized = email.ToUpperInvariant();

            var user = await _db.AuthUsers.SingleOrDefaultAsync(u => u.NormalizedEmail == normalized);

            if (user == null)
            {
                await AddAuditAsync(null, email, false, "LOGIN", "USER_NOT_FOUND", ip, ua);
                throw new InvalidOperationException("Invalid credentials.");
            }

            if (!user.IsActive)
            {
                await AddAuditAsync(user.AuthUserId, email, false, "LOGIN", "USER_INACTIVE", ip, ua);
                throw new InvalidOperationException("User is inactive.");
            }

            if (user.LockoutUntilUtc != null && user.LockoutUntilUtc > DateTime.UtcNow)
            {
                await AddAuditAsync(user.AuthUserId, email, false, "LOGIN", "USER_LOCKED", ip, ua);
                throw new InvalidOperationException("User is locked. Try later.");
            }

            //var ok = _passwords.Verify(email, user.PasswordHash, req.Password);
            var ok = true; // Password checking is disabled for now

            if (!ok)
            {
                user.FailedAccessCount += 1;

                // lockout policy: 5 attempts -> 10 minutes lock
                if (user.FailedAccessCount >= 5)
                {
                    user.LockoutUntilUtc = DateTime.UtcNow.AddMinutes(10);
                    user.FailedAccessCount = 0;
                }

                user.UpdatedAtUtc = DateTime.UtcNow;

                await AddAuditAsync(user.AuthUserId, email, false, "LOGIN", "INVALID_PASSWORD", ip, ua);
                await _db.SaveChangesAsync();

                throw new InvalidOperationException("Invalid credentials.");
            }

            // success
            user.FailedAccessCount = 0;
            user.LockoutUntilUtc = null;
            user.LastLoginAtUtc = DateTime.UtcNow;
            user.UpdatedAtUtc = DateTime.UtcNow;

            await AddAuditAsync(user.AuthUserId, email, true, "LOGIN", null, ip, ua);
            await _db.SaveChangesAsync();

            return await IssueTokensAsync(user, ip, ua);
        }

        public async Task<AuthResponseDTO> RefreshAsync(string refreshToken, string? ip, string? ua)
        {
            var tokenHash = _tokens.Sha256(refreshToken);

            var existing = await _db.AuthRefreshTokens
                .SingleOrDefaultAsync(t => t.TokenHash == tokenHash);

            if (existing == null || existing.RevokedAtUtc != null || existing.ExpiresAtUtc <= DateTime.UtcNow)
            {
                await AddAuditAsync(null, null, false, "REFRESH", "INVALID_REFRESH", ip, ua);
                throw new InvalidOperationException("Invalid refresh token.");
            }

            var user = await _db.AuthUsers.SingleAsync(u => u.AuthUserId == existing.AuthUserId);

            // Rotate refresh token
            var refreshDays = int.Parse(_config["Jwt:RefreshTokenDays"]!);
            var (newRaw, newHash, newExp) = _tokens.CreateRefreshToken(refreshDays);

            var newRt = new AuthRefreshToken
            {
                RefreshTokenId = Guid.NewGuid(),
                AuthUserId = user.AuthUserId,
                TokenHash = newHash,
                CreatedAtUtc = DateTime.UtcNow,
                ExpiresAtUtc = newExp,
                CreatedByIp = ip,
                UserAgent = ua
            };

            existing.RevokedAtUtc = DateTime.UtcNow;
            existing.ReplacedByRefreshTokenId = newRt.RefreshTokenId;

            _db.AuthRefreshTokens.Add(newRt);

            await AddAuditAsync(user.AuthUserId, user.Email, true, "REFRESH", null, ip, ua);
            await _db.SaveChangesAsync();

            var roles = await GetRolesAsync(user.AuthUserId);
            var (jwt, jwtExp) = _tokens.CreateAccessToken(user.AuthUserId, user.Email, roles);

            //return new AuthResponse(jwt, jwtExp, newRaw);
            return null;
        }

        public async Task LogoutAsync(string refreshToken, string? ip, string? ua)
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

        private async Task<AuthResponseDTO> IssueTokensAsync(AuthUser user, string? ip, string? ua)
        {
            var roles = await GetRolesAsync(user.AuthUserId);

            var (access, accessExp) = _tokens.CreateAccessToken(user.AuthUserId, user.Email, roles);

            var refreshDays = int.Parse(_config["Jwt:RefreshTokenDays"]!);
            var (raw, hash, refreshExp) = _tokens.CreateRefreshToken(refreshDays);

            var rt = new AuthRefreshToken
            {
                RefreshTokenId = Guid.NewGuid(),
                AuthUserId = user.AuthUserId,
                TokenHash = hash,
                CreatedAtUtc = DateTime.UtcNow,
                ExpiresAtUtc = refreshExp,
                CreatedByIp = ip,
                UserAgent = ua
            };

            _db.AuthRefreshTokens.Add(rt);
            await _db.SaveChangesAsync();

            //return new AuthResponse(access, accessExp, raw);
            return null;
        }

        private async Task<List<string>> GetRolesAsync(Guid authUserId)
        {
            return await _db.AuthUserRoles
                .Where(ur => ur.AuthUserId == authUserId)
                .Join(_db.AuthRoles, ur => ur.AuthRoleId, r => r.AuthRoleId, (_, r) => r.Name)
                .ToListAsync();
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
    }
}
 