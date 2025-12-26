using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GivealittleV2.Domain.Models.OTP
{
    public sealed class OtpOptions
    {
        public int OtpLength { get; set; } = 6;
        public int ExpiryMinutes { get; set; } = 10;
        public int MaxAttempts { get; set; } = 5;

        // Throttle resend
        public int ResendCooldownSeconds { get; set; } = 60;

        // Lock after too many attempts
        public int LockMinutes { get; set; } = 15;

        // A secret "pepper" stored in appsettings / KeyVault (NOT in DB)
        public string Pepper { get; set; } = default!;
    }
}
