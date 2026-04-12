using RiverSentry.Contracts.DTOs;

namespace RiverSentry.UI.Shared.Services;

/// <summary>
/// Client for the River Sentry API — shared between web and mobile.
/// </summary>
public interface IRiverSentryApiClient
{
    Task<IReadOnlyList<DeviceDto>> GetDevicesAsync(CancellationToken ct = default);
    Task<DeviceDto?> GetDeviceAsync(Guid id, CancellationToken ct = default);
    Task<DeviceStatusDto?> GetDeviceStatusAsync(Guid deviceId, CancellationToken ct = default);
    Task<IReadOnlyList<MapMarkerDto>> GetMapMarkersAsync(CancellationToken ct = default);
    Task<IReadOnlyList<AlertEventDto>> GetRecentAlertsAsync(int count = 50, CancellationToken ct = default);
    Task<IReadOnlyList<AlertEventDto>> GetActiveAlertsAsync(CancellationToken ct = default);
    Task AcknowledgeAlertAsync(Guid alertId, CancellationToken ct = default);
    Task<DashboardStats> GetDashboardStatsAsync(CancellationToken ct = default);
    Task<IReadOnlyList<DeviceFamilyDto>> GetDeviceFamiliesAsync(CancellationToken ct = default);
}
