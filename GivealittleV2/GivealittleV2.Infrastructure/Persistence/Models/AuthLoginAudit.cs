using System;
using System.Collections.Generic;

namespace GivealittleV2.Infrastructure.Persistence.Models;

public partial class AuthLoginAudit
{
    public long AuthLoginAuditId { get; set; }

    public DateTime OccurredAtUtc { get; set; }

    public Guid? AuthUserId { get; set; }

    public string? Email { get; set; }

    public bool Success { get; set; }

    public string EventType { get; set; } = null!;

    public string? FailureReason { get; set; }

    public string? IpAddress { get; set; }

    public string? UserAgent { get; set; }
}
