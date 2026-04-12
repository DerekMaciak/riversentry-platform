using RiverSentry.Domain.Entities;

namespace RiverSentry.Application.Interfaces;

public interface IDeviceStatusRepository
{
    Task<DeviceStatus?> GetLatestAsync(Guid deviceId, CancellationToken ct = default);
    Task<IReadOnlyList<DeviceStatus>> GetHistoryAsync(Guid deviceId, int count = 50, CancellationToken ct = default);
    Task AddAsync(DeviceStatus status, CancellationToken ct = default);
}
