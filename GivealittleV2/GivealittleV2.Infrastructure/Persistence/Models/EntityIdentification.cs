using System;
using System.Collections.Generic;

namespace GivealittleV2.Infrastructure.Persistence.Models;

public partial class EntityIdentification
{
    public Guid Id { get; set; }

    public byte[]? DrivingLicenseNumer { get; set; }

    public byte[]? DrivingLicenseVersion { get; set; }

    public byte[]? PassportNumber { get; set; }

    public byte[]? PassportExpiryDate { get; set; }

    public Guid DocumentId { get; set; }

    public virtual EntityDocument Document { get; set; } = null!;

    public virtual Entity IdNavigation { get; set; } = null!;
}
