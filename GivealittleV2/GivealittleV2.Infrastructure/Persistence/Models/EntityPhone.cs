using System;
using System.Collections.Generic;

namespace GivealittleV2.Infrastructure.Persistence.Models;

public partial class EntityPhone
{
    public Guid Id { get; set; }

    public string Number { get; set; } = null!;

    public bool IsDefault { get; set; }

    public Guid EntityId { get; set; }

    public virtual Entity Entity { get; set; } = null!;
}
