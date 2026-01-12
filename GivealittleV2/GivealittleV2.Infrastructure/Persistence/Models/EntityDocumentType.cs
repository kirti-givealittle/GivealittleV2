using System;
using System.Collections.Generic;

namespace GivealittleV2.Infrastructure.Persistence.Models;

public partial class EntityDocumentType
{
    public Guid Id { get; set; }

    public string DocumentType { get; set; } = null!;

    public byte[]? Description { get; set; }

    public virtual ICollection<EntityDocument> EntityDocuments { get; set; } = new List<EntityDocument>();
}
