using System;
using System.Collections.Generic;

namespace GivealittleV2.Infrastructure.Persistence.Models;

public partial class EntityRole
{
    public Guid Id { get; set; }

    public Guid RoleId { get; set; }

    public Guid EntityId { get; set; }

    public virtual Entity Entity { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
