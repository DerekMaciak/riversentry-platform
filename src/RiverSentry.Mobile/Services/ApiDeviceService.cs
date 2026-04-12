using RiverSentry.Contracts.DTOs;
using RiverSentry.UI.Shared.Services;

namespace RiverSentry.Mobile.Services;

/// <summary>
/// Device service that fetches data from the RiverSentry API.
/// </summary>
public class ApiDeviceService : IDeviceService
{
    private readonly IRiverSentryApiClient _apiClient;

    public ApiDeviceService(IRiverSentryApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<IEnumerable<DeviceDto>> GetAllDevicesAsync()
    {
        return await _apiClient.GetDevicesAsync();
    }

    public async Task<DeviceDto?> GetDeviceByIdAsync(Guid id)
    {
        var devices = await _apiClient.GetDevicesAsync();
        return devices.FirstOrDefault(d => d.Id == id);
    }

    public async Task<IEnumerable<DeviceDto>> GetDevicesByFamilyAsync(string familyName)
    {
        var devices = await _apiClient.GetDevicesAsync();
        return devices.Where(d => d.FamilyName == familyName);
    }

    public async Task<IEnumerable<DeviceDto>> GetAlarmingDevicesAsync()
    {
        var devices = await _apiClient.GetDevicesAsync();
        return devices.Where(d => d.IsAlarming);
    }
}
