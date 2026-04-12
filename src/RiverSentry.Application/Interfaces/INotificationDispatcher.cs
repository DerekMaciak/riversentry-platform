using RiverSentry.Domain.Entities;

namespace RiverSentry.Application.Interfaces;

/// <summary>
/// Dispatches push notifications to subscribed users.
/// </summary>
public interface INotificationDispatcher
{
    Task SendAlertNotificationAsync(AlertEvent alert, Device device, CancellationToken ct = default);
}
