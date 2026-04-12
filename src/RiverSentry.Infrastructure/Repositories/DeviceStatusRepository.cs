using Microsoft.EntityFrameworkCore;
using RiverSentry.Application.Interfaces;
using RiverSentry.Domain.Entities;
using RiverSentry.Infrastructure.Data;

namespace RiverSentry.Infrastructure.Repositories;

public class DeviceStatusRepository : IDeviceStatusRepository
{
    private readonly RiverSentryDbContext _db;

    public DeviceStatusRepository(RiverSentryDbContext db) => _db = db;

    public async Task<DeviceStatus?> GetLatestAsync(Guid deviceId, CancellationToken ct = default)
        => await _db.DeviceStatuses
            .AsNoTracking()
            .Where(s => s.DeviceId == deviceId)
            .OrderByDescending(s => s.ReceivedAt)
            .FirstOrDefaultAsync(ct);

    public async Task<IReadOnlyList<DeviceStatus>> GetHistoryAsync(Guid deviceId, int count = 50, CancellationToken ct = default)
        => await _db.DeviceStatuses
            .AsNoTracking()
            .Where(s => s.DeviceId == deviceId)
            .OrderByDescending(s => s.ReceivedAt)
            .Take(count)
            .ToListAsync(ct);

    public async Task AddAsync(DeviceStatus status, CancellationToken ct = default)
    {
        _db.DeviceStatuses.Add(status);
        await _db.SaveChangesAsync(ct);
    }
}
