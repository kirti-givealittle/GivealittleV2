using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GivealittleV2.Domain.Models.OTP
{
    public class OtpRecordDTO
    {
        public string Id { get; set; } = null!;

        public string UserKey { get; set; } = null!;

        public int Purpose { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public DateTime ExpiresAtUtc { get; set; }

        public int FailedAttempts { get; set; }

        public int MaxAttempts { get; set; }

        public DateTime? LockedUntilUtc { get; set; }

        public DateTime? UsedAtUtc { get; set; }

        public DateTime? NextResendAllowedAtUtc { get; set; }
    }

}
