using RiverSentry.Contracts.DTOs;

namespace RiverSentry.UI.Shared.Services;

/// <summary>
/// Service for retrieving device information.
/// </summary>
public interface IDeviceService
{
    /// <summary>
    /// Get all devices.
    /// </summary>
    Task<IEnumerable<DeviceDto>> GetAllDevicesAsync();

    /// <summary>
    /// Get a device by its ID.
    /// </summary>
    Task<DeviceDto?> GetDeviceByIdAsync(Guid id);

    /// <summary>
    /// Get all devices belonging to a family.
    /// </summary>
    Task<IEnumerable<DeviceDto>> GetDevicesByFamilyAsync(string familyName);

    /// <summary>
    /// Get all devices that are currently alarming.
    /// </summary>
    Task<IEnumerable<DeviceDto>> GetAlarmingDevicesAsync();
}
