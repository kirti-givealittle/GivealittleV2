using System;
using System.Collections.Generic;

namespace GivealittleV2.Infrastructure.Persistence.Models;

public partial class Charity
{
    public Guid Id { get; set; }

    public string Ccnumber { get; set; } = null!;

    public virtual Entity IdNavigation { get; set; } = null!;
}
