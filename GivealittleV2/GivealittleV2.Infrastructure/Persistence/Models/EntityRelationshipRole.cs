using System;
using System.Collections.Generic;

namespace GivealittleV2.Infrastructure.Persistence.Models;

public partial class EntityRelationshipRole
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public Guid EntityRelationshipId { get; set; }

    public virtual EntityRelationship EntityRelationship { get; set; } = null!;
}
