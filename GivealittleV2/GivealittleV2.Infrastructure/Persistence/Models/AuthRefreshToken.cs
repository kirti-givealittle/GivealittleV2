using System;
using System.Collections.Generic;

namespace GivealittleV2.Infrastructure.Persistence.Models;

public partial class AuthRefreshToken
{
    public Guid RefreshTokenId { get; set; }

    public Guid AuthUserId { get; set; }

    public byte[] TokenHash { get; set; } = null!;

    public DateTime CreatedAtUtc { get; set; }

    public DateTime ExpiresAtUtc { get; set; }

    public DateTime? RevokedAtUtc { get; set; }

    public Guid? ReplacedByRefreshTokenId { get; set; }

    public string? CreatedByIp { get; set; }

    public string? UserAgent { get; set; }

    public virtual EntityEmail AuthUser { get; set; } = null!;
}
