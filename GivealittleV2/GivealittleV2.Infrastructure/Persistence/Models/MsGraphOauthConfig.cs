using System;
using System.Collections.Generic;

namespace GivealittleV2.Infrastructure.Persistence.Models;

public partial class MsGraphOauthConfig
{
    public Guid MsGraphConfigId { get; set; }

    public string TenantId { get; set; } = null!;

    public string ClientId { get; set; } = null!;

    public string ClientSecret { get; set; } = null!;

    public string? RefreshToken { get; set; }

    public string? AccessToken { get; set; }

    public DateTime? AccessTokenExpiresAtUtc { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime UpdatedAtUtc { get; set; }
}
