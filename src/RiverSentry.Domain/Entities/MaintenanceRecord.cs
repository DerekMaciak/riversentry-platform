using RiverSentry.Domain.Enums;

namespace RiverSentry.Domain.Entities;

/// <summary>
/// A maintenance/service record for a device.
/// </summary>
public class MaintenanceRecord
{
    public Guid Id { get; set; }

    /// <summary>The device this record belongs to</summary>
    public Guid DeviceId { get; set; }

    /// <summary>Type of service performed</summary>
    public ServiceType ServiceType { get; set; }

    /// <summary>When the service was performed</summary>
    public DateTime PerformedAt { get; set; }

    /// <summary>Name of technician who performed the service</summary>
    public string? PerformedBy { get; set; }

    /// <summary>Service notes/description</summary>
    public string? Notes { get; set; }

    /// <summary>Firmware version before the service (if applicable)</summary>
    public string? FirmwareVersionBefore { get; set; }

    /// <summary>Firmware version after the service (if applicable)</summary>
    public string? FirmwareVersionAfter { get; set; }

    /// <summary>GPS latitude if location was updated</summary>
    public double? NewLatitude { get; set; }

    /// <summary>GPS longitude if location was updated</summary>
    public double? NewLongitude { get; set; }

    /// <summary>When this record was created</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property
    public Device Device { get; set; } = null!;
}
