using System;
using System.Collections.Generic;

namespace GivealittleV2.Infrastructure.Persistence.Models;

public partial class Group
{
    public Guid Id { get; set; }

    public virtual Entity IdNavigation { get; set; } = null!;
}
