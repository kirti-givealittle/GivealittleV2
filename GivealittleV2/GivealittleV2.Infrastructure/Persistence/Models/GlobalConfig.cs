using System;
using System.Collections.Generic;

namespace GivealittleV2.Infrastructure.Persistence.Models;

public partial class GlobalConfig
{
    public Guid Id { get; set; }

    public string Key { get; set; } = null!;

    public string? Value { get; set; }

    public string KeyGroup { get; set; } = null!;

    public DateTime CreatedAtUtc { get; set; }

    public DateTime UpdatedAtUtc { get; set; }
}
