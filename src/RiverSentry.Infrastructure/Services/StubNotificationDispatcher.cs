using Microsoft.Extensions.Logging;
using RiverSentry.Application.Interfaces;
using RiverSentry.Domain.Entities;

namespace RiverSentry.Infrastructure.Services;

/// <summary>
/// Stub notification dispatcher — logs instead of sending push notifications.
/// Replace with Azure Notification Hubs implementation in Phase 3.
/// </summary>
public class StubNotificationDispatcher : INotificationDispatcher
{
    private readonly ILogger<StubNotificationDispatcher> _logger;

    public StubNotificationDispatcher(ILogger<StubNotificationDispatcher> logger)
    {
        _logger = logger;
    }

    public Task SendAlertNotificationAsync(AlertEvent alert, Device device, CancellationToken ct = default)
    {
        _logger.LogInformation(
            "STUB: Would send push notification for alert {AlertId} on device {DeviceName} ({AlarmType})",
            alert.Id, device.Name, alert.AlarmType);
        return Task.CompletedTask;
    }
}
