using System;
using System.Collections.Generic;

namespace GivealittleV2.Infrastructure.Persistence.Models;

public partial class Role
{
    public Guid Id { get; set; }

    public byte[] Name { get; set; } = null!;

    public virtual ICollection<EntityRole> EntityRoles { get; set; } = new List<EntityRole>();
}
