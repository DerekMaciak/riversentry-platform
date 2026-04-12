using RiverSentry.Application.Interfaces;
using RiverSentry.Contracts.DTOs;
using RiverSentry.Domain.Entities;
using RiverSentry.Domain.Enums;

namespace RiverSentry.Application.Services;

public class AlertService
{
    private readonly IAlertRepository _alertRepo;
    private readonly IDeviceRepository _deviceRepo;
    private readonly INotificationDispatcher _notificationDispatcher;
    private readonly IAlertBroadcaster _alertBroadcaster;

    public AlertService(
        IAlertRepository alertRepo,
        IDeviceRepository deviceRepo,
        INotificationDispatcher notificationDispatcher,
        IAlertBroadcaster alertBroadcaster)
    {
        _alertRepo = alertRepo;
        _deviceRepo = deviceRepo;
        _notificationDispatcher = notificationDispatcher;
        _alertBroadcaster = alertBroadcaster;
    }

    public async Task<IReadOnlyList<AlertEventDto>> GetRecentAlertsAsync(int count = 50, CancellationToken ct = default)
    {
        var alerts = await _alertRepo.GetRecentAsync(count, ct);
        return alerts.Select(a => MapToDto(a)).ToList();
    }

    public async Task<IReadOnlyList<AlertEventDto>> GetActiveAlertsAsync(CancellationToken ct = default)
    {
        var alerts = await _alertRepo.GetActiveAsync(ct);
        return alerts.Select(a => MapToDto(a)).ToList();
    }

    /// <summary>
    /// Called when a device raises an alert. Persists, broadcasts, and sends push notifications.
    /// </summary>
    public async Task<AlertEventDto> RaiseAlertAsync(
        Guid deviceId, AlarmType alarmType, AlertSeverity severity, string? description,
        CancellationToken ct = default)
    {
        var device = await _deviceRepo.GetByIdAsync(deviceId, ct)
            ?? throw new InvalidOperationException($"Device {deviceId} not found");

        var alert = new AlertEvent
        {
            Id = Guid.NewGuid(),
            DeviceId = deviceId,
            AlarmType = alarmType,
            Severity = severity,
            Description = description,
            TriggeredAt = DateTime.UtcNow
        };

        await _alertRepo.AddAsync(alert, ct);

        // Update device state
        device.State = alarmType == AlarmType.HighWaterAlarm ? DeviceState.AlarmWater : DeviceState.AlarmUpstream;
        await _deviceRepo.UpdateAsync(device, ct);

        var dto = MapToDto(alert, device.Name);

        // Broadcast to connected clients and send push notifications
        await _alertBroadcaster.BroadcastAlertAsync(dto, ct);
        await _notificationDispatcher.SendAlertNotificationAsync(alert, device, ct);

        return dto;
    }

    private static AlertEventDto MapToDto(AlertEvent a, string? deviceName = null) => new()
    {
        Id = a.Id,
        DeviceId = a.DeviceId,
        DeviceName = deviceName ?? a.Device?.Name ?? string.Empty,
        AlarmType = a.AlarmType,
        Severity = a.Severity,
        Description = a.Description,
        TriggeredAt = a.TriggeredAt,
        ResolvedAt = a.ResolvedAt,
        IsActive = a.IsActive
    };
}
