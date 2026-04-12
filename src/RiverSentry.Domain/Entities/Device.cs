using RiverSentry.Domain.Enums;

namespace RiverSentry.Domain.Entities;

/// <summary>
/// A River Sentry device installation.
/// </summary>
public class Device
{
    public Guid Id { get; set; }

    /// <summary>Friendly device name (e.g., "Main Bridge Sensor")</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Hardware MAC address (12 hex chars, unique identifier from firmware)</summary>
    public string MacAddress { get; set; } = string.Empty;

    /// <summary>Product type (RS-1A, RS-1B, etc.)</summary>
    public int ProductTypeId { get; set; }

    /// <summary>Family/group this device belongs to</summary>
    public Guid? FamilyId { get; set; }

    /// <summary>Notes about this device</summary>
    public string? Notes { get; set; }

    #region Location & Calibration

    /// <summary>Latitude in decimal degrees</summary>
    public double Latitude { get; set; }

    /// <summary>Longitude in decimal degrees</summary>
    public double Longitude { get; set; }

    /// <summary>Altitude in meters (optional)</summary>
    public double? Altitude { get; set; }

    /// <summary>Device elevation in feet</summary>
    public double? Elevation { get; set; }

    /// <summary>Water elevation reference point in feet</summary>
    public double? WaterElevation { get; set; }

    /// <summary>Height above water in feet</summary>
    public double? HeightAboveWater { get; set; }

    /// <summary>Human-readable location description (e.g., "Under Main Street Bridge")</summary>
    public string? LocationDescription { get; set; }

    #endregion

    #region Owner Information

    /// <summary>Device owner/contact name</summary>
    public string? OwnerName { get; set; }

    /// <summary>Owner phone number</summary>
    public string? OwnerPhone { get; set; }

    /// <summary>Owner email address</summary>
    public string? OwnerEmail { get; set; }

    /// <summary>Owner address line 1</summary>
    public string? OwnerAddress1 { get; set; }

    /// <summary>Owner address line 2</summary>
    public string? OwnerAddress2 { get; set; }

    /// <summary>Owner city</summary>
    public string? OwnerCity { get; set; }

    /// <summary>Owner state</summary>
    public string? OwnerState { get; set; }

    /// <summary>Owner ZIP code</summary>
    public string? OwnerZip { get; set; }

    #endregion

    #region WiFi Configuration

    /// <summary>Configured WiFi SSID</summary>
    public string? WifiSsid { get; set; }

    /// <summary>Whether device is connected to WiFi</summary>
    public bool WifiConnected { get; set; }

    /// <summary>WiFi signal strength (RSSI in dBm)</summary>
    public int? WifiRssi { get; set; }

    /// <summary>Device IP address when connected</summary>
    public string? WifiIpAddress { get; set; }

    #endregion

    #region Device Status & Authentication

    /// <summary>Hashed API key for device-to-API authentication</summary>
    public string ApiKeyHash { get; set; } = string.Empty;

    /// <summary>Current operational state</summary>
    public DeviceState State { get; set; } = DeviceState.Unknown;

    /// <summary>Whether the device is currently reachable</summary>
    public bool IsOnline { get; set; }

    /// <summary>Current firmware version (e.g., "1.0.3")</summary>
    public string? FirmwareVersion { get; set; }

    /// <summary>Hardware version/revision (e.g., "V2.1")</summary>
    public string? HardwareVersion { get; set; }

    #endregion

    #region Timestamps

    /// <summary>When the device was physically installed</summary>
    public DateTime? InstalledAt { get; set; }

    /// <summary>When the device was last serviced/maintained</summary>
    public DateTime? LastServiceAt { get; set; }

    /// <summary>Last time we received a status update from this device</summary>
    public DateTime? LastStatusAt { get; set; }

    /// <summary>Record creation timestamp</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Record last updated timestamp</summary>
    public DateTime? UpdatedAt { get; set; }

    #endregion

    #region Navigation Properties

    public ProductType ProductType { get; set; } = null!;
    public Family? Family { get; set; }
    public ICollection<DeviceStatus> StatusHistory { get; set; } = new List<DeviceStatus>();
    public ICollection<AlertEvent> Alerts { get; set; } = new List<AlertEvent>();
    public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    public ICollection<MaintenanceRecord> MaintenanceHistory { get; set; } = new List<MaintenanceRecord>();

    #endregion

    #region Computed Properties

    /// <summary>Whether the device is in any alarm state</summary>
    public bool IsAlarming => State is DeviceState.AlarmWater
        or DeviceState.AlarmUpstream
        or DeviceState.AlarmSilent
        or DeviceState.AlarmDrill;

    /// <summary>WiFi signal strength indicator (0-4 bars based on RSSI)</summary>
    public int WifiSignalBars
    {
        get
        {
            if (!WifiConnected || !WifiRssi.HasValue) return 0;
            var rssi = WifiRssi.Value;
            if (rssi >= -50) return 4; // Excellent
            if (rssi >= -60) return 3; // Good
            if (rssi >= -70) return 2; // Fair
            if (rssi >= -80) return 1; // Weak
            return 0; // Very weak
        }
    }

    #endregion
}
