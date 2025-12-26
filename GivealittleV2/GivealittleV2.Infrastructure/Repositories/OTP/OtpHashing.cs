using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GivealittleV2.Infrastructure.Repositories.OTP
{
    public static class OtpHashing
    {
        public static (byte[] hash, byte[] salt) HashOtp(string otp, string pepper)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16);

            // Combine OTP + pepper (pepper is app secret)
            var input = Encoding.UTF8.GetBytes($"{otp}:{pepper}");

            // PBKDF2
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password: input,
                salt: salt,
                iterations: 100_000,
                hashAlgorithm: HashAlgorithmName.SHA256,
                outputLength: 32);

            return (hash, salt);
        }

        public static byte[] HashOtpWithSalt(string otp, string pepper, byte[] salt)
        {
            var input = Encoding.UTF8.GetBytes($"{otp}:{pepper}");

            return Rfc2898DeriveBytes.Pbkdf2(
                password: input,
                salt: salt,
                iterations: 100_000,
                hashAlgorithm: HashAlgorithmName.SHA256,
                outputLength: 32);
        }

        public static bool FixedTimeEquals(byte[] a, byte[] b)
            => CryptographicOperations.FixedTimeEquals(a, b);
    }
}
