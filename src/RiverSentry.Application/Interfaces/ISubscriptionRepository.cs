using RiverSentry.Domain.Entities;

namespace RiverSentry.Application.Interfaces;

public interface ISubscriptionRepository
{
    Task<IReadOnlyList<Subscription>> GetByUserAsync(Guid userId, CancellationToken ct = default);
    Task<IReadOnlyList<Subscription>> GetByDeviceAsync(Guid deviceId, CancellationToken ct = default);
    Task<Subscription?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(Subscription subscription, CancellationToken ct = default);
    Task UpdateAsync(Subscription subscription, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
