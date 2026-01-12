using System;
using System.Collections.Generic;

namespace GivealittleV2.Infrastructure.Persistence.Models;

public partial class OtpRecord
{
    public Guid Id { get; set; }

    public string UserEmail { get; set; } = null!;

    public int Purpose { get; set; }

    public byte[] OtpHash { get; set; } = null!;

    public byte[] Salt { get; set; } = null!;

    public DateTime CreatedAtUtc { get; set; }

    public DateTime ExpiresAtUtc { get; set; }

    public int FailedAttempts { get; set; }

    public int MaxAttempts { get; set; }

    public DateTime? LockedUntilUtc { get; set; }

    public DateTime? UsedAtUtc { get; set; }

    public DateTime? NextResendAllowedAtUtc { get; set; }
}
