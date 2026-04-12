using RiverSentry.Domain.Enums;

namespace RiverSentry.Domain.Entities;

/// <summary>
/// A point-in-time status snapshot from a device.
/// </summary>
public class DeviceStatus
{
    public Guid Id { get; set; }

    /// <summary>The device this status belongs to</summary>
    public Guid DeviceId { get; set; }

    /// <summary>Operational state at time of report</summary>
    public DeviceState State { get; set; }

    /// <summary>Main power voltage</summary>
    public double MainVolts { get; set; }

    /// <summary>Battery voltage</summary>
    public double BatteryVolts { get; set; }

    /// <summary>Water witch 1 status (true = water detected)</summary>
    public bool WaterWitch1 { get; set; }

    /// <summary>Water witch 2 status (true = water detected)</summary>
    public bool WaterWitch2 { get; set; }

    /// <summary>Whether WiFi is connected</summary>
    public bool WifiConnected { get; set; }

    /// <summary>NTP time sync status</summary>
    public bool NtpSync { get; set; }

    /// <summary>When the API received this status</summary>
    public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;

    // Navigation property
    public Device Device { get; set; } = null!;

    /// <summary>Whether any water sensor is triggered</summary>
    public bool HasWaterDetection => WaterWitch1 || WaterWitch2;
}
