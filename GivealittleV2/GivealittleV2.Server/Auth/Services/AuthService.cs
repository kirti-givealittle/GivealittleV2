using GivealittleV2.Infrastructure.Persistence.Models;
using GivealittleV2.Server.Auth.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GivealittleV2.Server.Auth.Services
{
    public sealed class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _db;
        private readonly IPasswordService _passwords;
        private readonly ITokenService _tokens;
        private readonly IConfiguration _config;

        public AuthService(ApplicationDbContext db, IPasswordService passwords, ITokenService tokens, IConfiguration config)
        {
            _db = db;
            _passwords = passwords;
            _tokens = tokens;
            _config = config;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest req, string? ip, string? ua)
        {
            var email = req.Email.Trim();
            var normalized = email.ToUpperInvariant();

            var exists = await _db.AuthUsers.AnyAsync(u => u.NormalizedEmail == normalized);
            if (exists) throw new InvalidOperationException("Email already registered.");

            var authUserId = Guid.NewGuid();
            var user = new AuthUser
            {
                AuthUserId = authUserId,
                Email = email,
                NormalizedEmail = normalized,
                PasswordHash = _passwords.HashPassword(email, req.Password),
                EmailConfirmed = false,
                IsActive = true,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow,
                FailedAccessCount = 0
            };

            _db.AuthUsers.Add(user);

            // Assign default role "User" (optional)
            var userRole = await _db.AuthRoles.SingleOrDefaultAsync(r => r.NormalizedName == "USER");
            if (userRole != null)
            {
                _db.AuthUserRoles.Add(new AuthUserRole
                {
                    AuthUserId = authUserId,
                    AuthRoleId = userRole.AuthRoleId,
                    AssignedAtUtc = DateTime.UtcNow
                });
            }

            await AddAuditAsync(authUserId, email, success: true, "REGISTER", null, ip, ua);

            await _db.SaveChangesAsync();
            return await IssueTokensAsync(user, ip, ua);
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest req, string? ip, string? ua)
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

            var ok = _passwords.Verify(email, user.PasswordHash, req.Password);

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

        public async Task<AuthResponse> RefreshAsync(string refreshToken, string? ip, string? ua)
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

            return new AuthResponse(jwt, jwtExp, newRaw);
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

        private async Task<AuthResponse> IssueTokensAsync(AuthUser user, string? ip, string? ua)
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

            return new AuthResponse(access, accessExp, raw);
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
 