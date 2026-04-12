using RiverSentry.Domain.Entities;
using RiverSentry.Domain.Enums;

namespace RiverSentry.Application.Interfaces;

public interface IMaintenanceRepository
{
    Task<MaintenanceRecord?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<MaintenanceRecord>> GetByDeviceAsync(Guid deviceId, CancellationToken ct = default);
    Task<IReadOnlyList<MaintenanceRecord>> GetRecentAsync(int count = 50, CancellationToken ct = default);
    Task<IReadOnlyList<MaintenanceRecord>> GetByServiceTypeAsync(ServiceType serviceType, CancellationToken ct = default);
    Task<MaintenanceRecord?> GetLatestByDeviceAsync(Guid deviceId, CancellationToken ct = default);
    Task AddAsync(MaintenanceRecord record, CancellationToken ct = default);
    Task UpdateAsync(MaintenanceRecord record, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
