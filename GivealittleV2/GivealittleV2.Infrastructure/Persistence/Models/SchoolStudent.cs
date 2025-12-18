using System;
using System.Collections.Generic;

namespace GivealittleV2.Infrastructure.Persistence.Models;

public partial class SchoolStudent
{
    public Guid IndividualId { get; set; }

    public Guid SchoolId { get; set; }

    public Guid ClassId { get; set; }

    public virtual SchoolClass Class { get; set; } = null!;

    public virtual Individual Individual { get; set; } = null!;

    public virtual School School { get; set; } = null!;
}
