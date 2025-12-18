using System;
using System.Collections.Generic;

namespace GivealittleV2.Infrastructure.Persistence.Models;

public partial class BankAccount
{
    public Guid Id { get; set; }

    public short? AccountNumber { get; set; }

    public string? Reference { get; set; }

    public Guid? EntityId { get; set; }

    public virtual ICollection<BankAccountDocument> BankAccountDocuments { get; set; } = new List<BankAccountDocument>();

    public virtual Entity? Entity { get; set; }
}
