using Microsoft.EntityFrameworkCore;
using RiverSentry.Application.Interfaces;
using RiverSentry.Domain.Entities;
using RiverSentry.Domain.Enums;
using RiverSentry.Infrastructure.Data;

namespace RiverSentry.Infrastructure.Repositories;

public class MaintenanceRepository : IMaintenanceRepository
{
    private readonly RiverSentryDbContext _db;

    public MaintenanceRepository(RiverSentryDbContext db) => _db = db;

    public async Task<MaintenanceRecord?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _db.MaintenanceRecords
            .Include(m => m.Device)
            .FirstOrDefaultAsync(m => m.Id == id, ct);

    public async Task<IReadOnlyList<MaintenanceRecord>> GetByDeviceAsync(Guid deviceId, CancellationToken ct = default)
        => await _db.MaintenanceRecords
            .AsNoTracking()
            .Where(m => m.DeviceId == deviceId)
            .OrderByDescending(m => m.PerformedAt)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<MaintenanceRecord>> GetRecentAsync(int count = 50, CancellationToken ct = default)
        => await _db.MaintenanceRecords
            .AsNoTracking()
            .Include(m => m.Device)
            .OrderByDescending(m => m.PerformedAt)
            .Take(count)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<MaintenanceRecord>> GetByServiceTypeAsync(ServiceType serviceType, CancellationToken ct = default)
        => await _db.MaintenanceRecords
            .AsNoTracking()
            .Include(m => m.Device)
            .Where(m => m.ServiceType == serviceType)
            .OrderByDescending(m => m.PerformedAt)
            .ToListAsync(ct);

    public async Task<MaintenanceRecord?> GetLatestByDeviceAsync(Guid deviceId, CancellationToken ct = default)
        => await _db.MaintenanceRecords
            .AsNoTracking()
            .Where(m => m.DeviceId == deviceId)
            .OrderByDescending(m => m.PerformedAt)
            .FirstOrDefaultAsync(ct);

    public async Task AddAsync(MaintenanceRecord record, CancellationToken ct = default)
    {
        _db.MaintenanceRecords.Add(record);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(MaintenanceRecord record, CancellationToken ct = default)
    {
        _db.MaintenanceRecords.Update(record);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.MaintenanceRecords.FindAsync([id], ct);
        if (entity is not null)
        {
            _db.MaintenanceRecords.Remove(entity);
            await _db.SaveChangesAsync(ct);
        }
    }
}
