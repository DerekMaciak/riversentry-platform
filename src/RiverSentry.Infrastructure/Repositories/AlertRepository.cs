using Microsoft.EntityFrameworkCore;
using RiverSentry.Application.Interfaces;
using RiverSentry.Domain.Entities;
using RiverSentry.Infrastructure.Data;

namespace RiverSentry.Infrastructure.Repositories;

public class AlertRepository : IAlertRepository
{
    private readonly RiverSentryDbContext _db;

    public AlertRepository(RiverSentryDbContext db) => _db = db;

    public async Task<IReadOnlyList<AlertEvent>> GetRecentAsync(int count = 50, CancellationToken ct = default)
        => await _db.AlertEvents
            .AsNoTracking()
            .Include(a => a.Device)
            .OrderByDescending(a => a.TriggeredAt)
            .Take(count)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<AlertEvent>> GetActiveAsync(CancellationToken ct = default)
        => await _db.AlertEvents
            .AsNoTracking()
            .Include(a => a.Device)
            .Where(a => a.ResolvedAt == null)
            .OrderByDescending(a => a.TriggeredAt)
            .ToListAsync(ct);

    public async Task<AlertEvent?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _db.AlertEvents.Include(a => a.Device).FirstOrDefaultAsync(a => a.Id == id, ct);

    public async Task AddAsync(AlertEvent alert, CancellationToken ct = default)
    {
        _db.AlertEvents.Add(alert);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(AlertEvent alert, CancellationToken ct = default)
    {
        _db.AlertEvents.Update(alert);
        await _db.SaveChangesAsync(ct);
    }
}
