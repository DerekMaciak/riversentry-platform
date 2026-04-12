namespace RiverSentry.Domain.Enums;

/// <summary>
/// Type of maintenance service performed on a device.
/// </summary>
public enum ServiceType
{
    /// <summary>Initial device installation</summary>
    Installation = 1,

    /// <summary>Routine inspection</summary>
    Inspection = 2,

    /// <summary>Repair or fix</summary>
    Repair = 3,

    /// <summary>Firmware upgrade</summary>
    FirmwareUpdate = 4,

    /// <summary>Battery replacement</summary>
    BatteryReplacement = 5,

    /// <summary>Sensor calibration</summary>
    SensorCalibration = 6,

    /// <summary>Device relocated to new position</summary>
    Relocation = 7,

    /// <summary>Device taken offline/decommissioned</summary>
    Decommission = 8
}
