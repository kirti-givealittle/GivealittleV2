using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GivealittleV2.Application.Interfaces.Auth
{
    public interface ITokenService
    {
        (string token, DateTime expiresAtUtc) CreateAccessToken(Guid authUserId, string email, IEnumerable<string> roles);
        (string refreshTokenRaw, byte[] refreshTokenHash, DateTime expiresAtUtc) CreateRefreshToken(int days);
        byte[] Sha256(string input);
    }
}
