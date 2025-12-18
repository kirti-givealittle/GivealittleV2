using System;
using System.Collections.Generic;

namespace GivealittleV2.Infrastructure.Persistence.Models;

public partial class SchoolClass
{
    public Guid Id { get; set; }

    public int ClassYear { get; set; }

    public int ClassRoomNumber { get; set; }
}
