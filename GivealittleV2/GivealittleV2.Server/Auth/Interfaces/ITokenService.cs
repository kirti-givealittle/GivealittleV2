namespace GivealittleV2.Server.Auth.Interfaces
{
    public interface ITokenService
    {
        (string token, DateTime expiresAtUtc) CreateAccessToken(Guid authUserId, string email, IEnumerable<string> roles);
        (string refreshTokenRaw, byte[] refreshTokenHash, DateTime expiresAtUtc) CreateRefreshToken(int days);
        byte[] Sha256(string input);
    }
}
