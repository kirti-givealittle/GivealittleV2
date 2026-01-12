using System;
using System.Collections.Generic;

namespace GivealittleV2.Infrastructure.Persistence.Models;

public partial class BankAccountDocument
{
    public Guid Id { get; set; }

    public Guid BankAccountId { get; set; }

    public virtual BankAccount BankAccount { get; set; } = null!;
}
