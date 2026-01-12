using System;
using System.Collections.Generic;

namespace GivealittleV2.Infrastructure.Persistence.Models;

public partial class Individual
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string LastName { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public bool? IsMinor { get; set; }

    public virtual Entity IdNavigation { get; set; } = null!;
}
