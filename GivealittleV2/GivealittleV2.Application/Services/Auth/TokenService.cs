using GivealittleV2.Application.Interfaces.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GivealittleV2.Application.Services.Auth
{
    public sealed class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        public TokenService(IConfiguration config) => _config = config;

        public (string token, DateTime expiresAtUtc) CreateAccessToken(Guid authUserId, string email, IEnumerable<string> roles)
        {
            var jwt = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwt["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow.AddMinutes(int.Parse(jwt["AccessTokenMinutes"]!));

            var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, authUserId.ToString()),
            new(JwtRegisteredClaimNames.Email, email),
            new(ClaimTypes.NameIdentifier, authUserId.ToString())
        };

            foreach (var r in roles)
                claims.Add(new Claim(ClaimTypes.Role, r));

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            return (new JwtSecurityTokenHandler().WriteToken(token), expires);
        }

        public (string refreshTokenRaw, byte[] refreshTokenHash, DateTime expiresAtUtc) CreateRefreshToken(int days)
        {
            var bytes = RandomNumberGenerator.GetBytes(64); // 512 bits
            var raw = Convert.ToBase64String(bytes);
            var hash = Sha256(raw);
            var expires = DateTime.UtcNow.AddDays(days);
            return (raw, hash, expires);
        }

        public byte[] Sha256(string input)
            => SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(input));
    }
}
