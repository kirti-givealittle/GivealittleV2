using System;
using System.Collections.Generic;

namespace GivealittleV2.Infrastructure.Persistence.Models;

public partial class EntityDocument
{
    public Guid Id { get; set; }

    public Guid EntityId { get; set; }

    public Guid EntityDocumentTypeId { get; set; }

    public virtual Entity Entity { get; set; } = null!;

    public virtual EntityDocumentType EntityDocumentType { get; set; } = null!;
}
