using System;
using System.Collections.Generic;

namespace GivealittleV2.Infrastructure.Persistence.Models;

public partial class EmailProvider
{
    public Guid ProviderId { get; set; }

    public string ProviderKey { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime? SwitchedAtUtc { get; set; }

    public string? SwitchedBy { get; set; }

    public string? SwitchReason { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime UpdatedAtUtc { get; set; }
}
