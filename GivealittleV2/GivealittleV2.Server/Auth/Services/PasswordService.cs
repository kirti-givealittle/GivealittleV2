using GivealittleV2.Server.Auth.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace GivealittleV2.Server.Auth.Services
{
    public sealed class PasswordService : IPasswordService
    {
        private readonly PasswordHasher<string> _hasher = new();

        public string HashPassword(string email, string password)
            => _hasher.HashPassword(email, password);

        public bool Verify(string email, string hash, string password)
            => _hasher.VerifyHashedPassword(email, hash, password) is PasswordVerificationResult.Success
               or PasswordVerificationResult.SuccessRehashNeeded;
    }
}
