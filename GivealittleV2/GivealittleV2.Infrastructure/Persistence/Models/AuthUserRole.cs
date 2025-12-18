using System;
using System.Collections.Generic;

namespace GivealittleV2.Infrastructure.Persistence.Models;

public partial class AuthUserRole
{
    public Guid AuthUserId { get; set; }

    public Guid AuthRoleId { get; set; }

    public DateTime AssignedAtUtc { get; set; }

    public virtual AuthRole AuthRole { get; set; } = null!;

    public virtual AuthUser AuthUser { get; set; } = null!;
}
