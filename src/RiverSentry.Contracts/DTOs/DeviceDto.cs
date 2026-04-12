using RiverSentry.Domain.Enums;

namespace RiverSentry.Contracts.DTOs;

/// <summary>
/// Device information for display to end users.
/// </summary>
public class DeviceDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? ProductTypeCode { get; set; }
    public string? ProductTypeName { get; set; }
    public string? FamilyName { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double? Altitude { get; set; }
    public string? LocationDescription { get; set; }
    public string? Notes { get; set; }
    public DeviceState State { get; set; }
    public bool IsOnline { get; set; }
    public DateTime? LastStatusAt { get; set; }
    public DateTime? InstalledDate { get; set; }
    public string? FirmwareVersion { get; set; }
    public string? HardwareVersion { get; set; }

    public bool IsAlarming => State is DeviceState.AlarmWater
        or DeviceState.AlarmUpstream
        or DeviceState.AlarmSilent
        or DeviceState.AlarmDrill;
}

/// <summary>
/// Extended device information for the Tech page (admin view).
/// </summary>
public class DeviceTechDto : DeviceDto
{
    // Calibration
    public double? Elevation { get; set; }
    public double? WaterElevation { get; set; }
    public double? HeightAboveWater { get; set; }

    // Owner Information
    public string? OwnerName { get; set; }
    public string? OwnerPhone { get; set; }
    public string? OwnerEmail { get; set; }
    public string? OwnerAddress1 { get; set; }
    public string? OwnerAddress2 { get; set; }
    public string? OwnerCity { get; set; }
    public string? OwnerState { get; set; }
    public string? OwnerZip { get; set; }

    // WiFi Configuration
    public string? WifiSsid { get; set; }
    public bool WifiConnected { get; set; }
    public int? WifiRssi { get; set; }
    public string? WifiIpAddress { get; set; }
    public int WifiSignalBars { get; set; }

    // Timestamps
    public DateTime? InstalledAt { get; set; }
    public DateTime? LastServiceAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// Lightweight marker data for map display.
/// </summary>
public class MapMarkerDto
{
    public Guid DeviceId { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DeviceState State { get; set; }
    public bool IsOnline { get; set; }
    public string? FamilyName { get; set; }
    public string? ProductTypeCode { get; set; }
}
