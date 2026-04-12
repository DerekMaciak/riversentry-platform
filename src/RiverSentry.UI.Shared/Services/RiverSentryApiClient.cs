using System.Net.Http.Json;
using RiverSentry.Contracts.DTOs;

namespace RiverSentry.UI.Shared.Services;

public class RiverSentryApiClient : IRiverSentryApiClient
{
    private readonly HttpClient _http;

    public RiverSentryApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<IReadOnlyList<DeviceDto>> GetDevicesAsync(CancellationToken ct = default)
        => await _http.GetFromJsonAsync<List<DeviceDto>>("api/devices", ct) ?? [];

    public async Task<DeviceDto?> GetDeviceAsync(Guid id, CancellationToken ct = default)
        => await _http.GetFromJsonAsync<DeviceDto>($"api/devices/{id}", ct);

    public async Task<DeviceStatusDto?> GetDeviceStatusAsync(Guid deviceId, CancellationToken ct = default)
        => await _http.GetFromJsonAsync<DeviceStatusDto>($"api/devices/{deviceId}/status", ct);

    public async Task<IReadOnlyList<MapMarkerDto>> GetMapMarkersAsync(CancellationToken ct = default)
        => await _http.GetFromJsonAsync<List<MapMarkerDto>>("api/devices/markers", ct) ?? [];

    public async Task<IReadOnlyList<AlertEventDto>> GetRecentAlertsAsync(int count = 50, CancellationToken ct = default)
        => await _http.GetFromJsonAsync<List<AlertEventDto>>($"api/alerts?count={count}", ct) ?? [];

    public async Task<IReadOnlyList<AlertEventDto>> GetActiveAlertsAsync(CancellationToken ct = default)
        => await _http.GetFromJsonAsync<List<AlertEventDto>>("api/alerts/active", ct) ?? [];

    public async Task AcknowledgeAlertAsync(Guid alertId, CancellationToken ct = default)
        => await _http.PostAsync($"api/alerts/{alertId}/acknowledge", null, ct);

    public async Task<DashboardStats> GetDashboardStatsAsync(CancellationToken ct = default)
        => await _http.GetFromJsonAsync<DashboardStats>("api/dashboard/stats", ct) ?? new DashboardStats();

    public async Task<IReadOnlyList<DeviceFamilyDto>> GetDeviceFamiliesAsync(CancellationToken ct = default)
        => await _http.GetFromJsonAsync<List<DeviceFamilyDto>>("api/families", ct) ?? [];
}
