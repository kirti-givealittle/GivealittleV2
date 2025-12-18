using System;
using System.Collections.Generic;

namespace GivealittleV2.Infrastructure.Persistence.Models;

public partial class AuthRole
{
    public Guid AuthRoleId { get; set; }

    public string Name { get; set; } = null!;

    public string NormalizedName { get; set; } = null!;

    public DateTime CreatedAtUtc { get; set; }

    public virtual ICollection<AuthUserRole> AuthUserRoles { get; set; } = new List<AuthUserRole>();
}
