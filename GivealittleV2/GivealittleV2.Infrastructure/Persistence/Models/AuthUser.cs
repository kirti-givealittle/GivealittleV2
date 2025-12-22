using System;
using System.Collections.Generic;

namespace GivealittleV2.Infrastructure.Persistence.Models;

public partial class AuthUser
{
    public Guid AuthUserId { get; set; }

    public string Email { get; set; } = null!;

    public string NormalizedEmail { get; set; } = null!;

    public bool EmailConfirmed { get; set; }

    public bool IsActive { get; set; }

    public int FailedAccessCount { get; set; }

    public DateTime? LockoutUntilUtc { get; set; }

    public DateTime? LastLoginAtUtc { get; set; }

    public DateTime? PasswordChangedAtUtc { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime UpdatedAtUtc { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual ICollection<AuthRefreshToken> AuthRefreshTokens { get; set; } = new List<AuthRefreshToken>();

    public virtual ICollection<AuthUserRole> AuthUserRoles { get; set; } = new List<AuthUserRole>();
}
