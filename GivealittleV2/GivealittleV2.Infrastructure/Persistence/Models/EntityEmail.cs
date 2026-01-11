using System;
using System.Collections.Generic;

namespace GivealittleV2.Infrastructure.Persistence.Models;

public partial class EntityEmail
{
    public Guid Id { get; set; }

    public string EmailAddress { get; set; } = null!;

    public bool IsDefault { get; set; }

    public Guid EntityId { get; set; }

    public bool IsVerified { get; set; }

    public virtual ICollection<AuthRefreshToken> AuthRefreshTokens { get; set; } = new List<AuthRefreshToken>();

    public virtual Entity Entity { get; set; } = null!;
}
