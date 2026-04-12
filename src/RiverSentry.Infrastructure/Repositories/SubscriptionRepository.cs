using Microsoft.EntityFrameworkCore;
using RiverSentry.Application.Interfaces;
using RiverSentry.Domain.Entities;
using RiverSentry.Infrastructure.Data;

namespace RiverSentry.Infrastructure.Repositories;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly RiverSentryDbContext _db;

    public SubscriptionRepository(RiverSentryDbContext db) => _db = db;

    public async Task<IReadOnlyList<Subscription>> GetByUserAsync(Guid userId, CancellationToken ct = default)
        => await _db.Subscriptions
            .AsNoTracking()
            .Include(s => s.Device)
            .Where(s => s.UserId == userId)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<Subscription>> GetByDeviceAsync(Guid deviceId, CancellationToken ct = default)
        => await _db.Subscriptions
            .AsNoTracking()
            .Where(s => s.DeviceId == deviceId && s.IsActive)
            .ToListAsync(ct);

    public async Task<Subscription?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _db.Subscriptions.FindAsync([id], ct);

    public async Task AddAsync(Subscription subscription, CancellationToken ct = default)
    {
        _db.Subscriptions.Add(subscription);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Subscription subscription, CancellationToken ct = default)
    {
        _db.Subscriptions.Update(subscription);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.Subscriptions.FindAsync([id], ct);
        if (entity is not null)
        {
            _db.Subscriptions.Remove(entity);
            await _db.SaveChangesAsync(ct);
        }
    }
}
