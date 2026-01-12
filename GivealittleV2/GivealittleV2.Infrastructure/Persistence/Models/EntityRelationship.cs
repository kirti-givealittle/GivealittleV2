using System;
using System.Collections.Generic;

namespace GivealittleV2.Infrastructure.Persistence.Models;

public partial class EntityRelationship
{
    public Guid Id { get; set; }

    public Guid EntityId { get; set; }

    public Guid RelatedEntityId { get; set; }

    public virtual Entity Entity { get; set; } = null!;

    public virtual ICollection<EntityRelationshipRole> EntityRelationshipRoles { get; set; } = new List<EntityRelationshipRole>();

    public virtual Entity RelatedEntity { get; set; } = null!;
}
