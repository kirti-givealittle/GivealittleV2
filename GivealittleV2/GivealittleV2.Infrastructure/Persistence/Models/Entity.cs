using System;
using System.Collections.Generic;

namespace GivealittleV2.Infrastructure.Persistence.Models;

public partial class Entity
{
    public Guid Id { get; set; }

    public bool IsIndividual { get; set; }

    public string? Irdnumber { get; set; }

    public virtual ICollection<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();

    public virtual Business? Business { get; set; }

    public virtual Charity? Charity { get; set; }

    public virtual ICollection<EntityAddress> EntityAddresses { get; set; } = new List<EntityAddress>();

    public virtual ICollection<EntityDocument> EntityDocuments { get; set; } = new List<EntityDocument>();

    public virtual ICollection<EntityEmail> EntityEmails { get; set; } = new List<EntityEmail>();

    public virtual ICollection<EntityPhone> EntityPhones { get; set; } = new List<EntityPhone>();

    public virtual ICollection<EntityRelationship> EntityRelationshipEntities { get; set; } = new List<EntityRelationship>();

    public virtual ICollection<EntityRelationship> EntityRelationshipRelatedEntities { get; set; } = new List<EntityRelationship>();

    public virtual ICollection<EntityRole> EntityRoles { get; set; } = new List<EntityRole>();

    public virtual Group? Group { get; set; }

    public virtual Individual? Individual { get; set; }

    public virtual School? School { get; set; }
}
