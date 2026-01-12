using System;
using System.Collections.Generic;

namespace GivealittleV2.Infrastructure.Persistence.Models;

public partial class EntityAddress
{
    public Guid Id { get; set; }

    public bool? IsDefault { get; set; }

    public Guid EntityId { get; set; }

    public virtual Entity Entity { get; set; } = null!;
}
