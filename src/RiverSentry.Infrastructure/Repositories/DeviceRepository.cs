using Microsoft.EntityFrameworkCore;
using RiverSentry.Application.Interfaces;
using RiverSentry.Domain.Entities;
using RiverSentry.Infrastructure.Data;

namespace RiverSentry.Infrastructure.Repositories;

public class DeviceRepository : IDeviceRepository
{
    private readonly RiverSentryDbContext _db;

    public DeviceRepository(RiverSentryDbContext db) => _db = db;

    public async Task<IReadOnlyList<Device>> GetAllAsync(CancellationToken ct = default)
        => await _db.Devices
            .AsNoTracking()
            .Include(d => d.ProductType)
            .Include(d => d.Family)
            .OrderBy(d => d.Name)
            .ToListAsync(ct);

    public async Task<Device?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _db.Devices
            .Include(d => d.ProductType)
            .Include(d => d.Family)
            .FirstOrDefaultAsync(d => d.Id == id, ct);

    public async Task<Device?> GetByMacAddressAsync(string macAddress, CancellationToken ct = default)
        => await _db.Devices.FirstOrDefaultAsync(d => d.MacAddress == macAddress, ct);

    public async Task AddAsync(Device device, CancellationToken ct = default)
    {
        _db.Devices.Add(device);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Device device, CancellationToken ct = default)
    {
        _db.Devices.Update(device);
        await _db.SaveChangesAsync(ct);
    }
}
