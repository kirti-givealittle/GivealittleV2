using System;
using System.Collections.Generic;

namespace GivealittleV2.Infrastructure.Persistence.Models;

public partial class EmailTemplate
{
    public Guid TemplateId { get; set; }

    public string TemplateKey { get; set; } = null!;

    public string TemplateType { get; set; } = null!;

    public string SubjectTemplate { get; set; } = null!;

    public string BodyTemplate { get; set; } = null!;

    public bool IsHtml { get; set; }

    public bool IsActive { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime UpdatedAtUtc { get; set; }
}
