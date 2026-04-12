using RiverSentry.Application.Interfaces;
using RiverSentry.Contracts.DTOs;
using RiverSentry.Domain.Entities;

namespace RiverSentry.Application.Services;

public class DeviceService
{
    private readonly IDeviceRepository _deviceRepo;
    private readonly IDeviceStatusRepository _statusRepo;

    public DeviceService(IDeviceRepository deviceRepo, IDeviceStatusRepository statusRepo)
    {
        _deviceRepo = deviceRepo;
        _statusRepo = statusRepo;
    }

    public async Task<IReadOnlyList<DeviceDto>> GetAllDevicesAsync(CancellationToken ct = default)
    {
        var devices = await _deviceRepo.GetAllAsync(ct);
        return devices.Select(MapToDto).ToList();
    }

    public async Task<DeviceDto?> GetDeviceByIdAsync(Guid id, CancellationToken ct = default)
    {
        var device = await _deviceRepo.GetByIdAsync(id, ct);
        return device is null ? null : MapToDto(device);
    }

    public async Task<IReadOnlyList<MapMarkerDto>> GetMapMarkersAsync(CancellationToken ct = default)
    {
        var devices = await _deviceRepo.GetAllAsync(ct);
        return devices.Select(d => new MapMarkerDto
        {
            DeviceId = d.Id,
            Name = d.Name,
            Latitude = d.Latitude,
            Longitude = d.Longitude,
            State = d.State,
            IsOnline = d.IsOnline,
            FamilyName = d.Family?.Name,
            ProductTypeCode = d.ProductType?.Code
        }).ToList();
    }

    public async Task<DeviceStatusDto?> GetLatestStatusAsync(Guid deviceId, CancellationToken ct = default)
    {
        var status = await _statusRepo.GetLatestAsync(deviceId, ct);
        if (status is null) return null;

        return new DeviceStatusDto
        {
            DeviceId = status.DeviceId,
            State = status.State,
            MainVolts = status.MainVolts,
            BatteryVolts = status.BatteryVolts,
            WaterWitch1 = status.WaterWitch1,
            WaterWitch2 = status.WaterWitch2,
            WifiConnected = status.WifiConnected,
            ReceivedAt = status.ReceivedAt
        };
    }

    private static DeviceDto MapToDto(Device d) => new()
    {
        Id = d.Id,
        Name = d.Name,
        ProductTypeCode = d.ProductType?.Code,
        ProductTypeName = d.ProductType?.Name,
        FamilyName = d.Family?.Name,
        Latitude = d.Latitude,
        Longitude = d.Longitude,
        Altitude = d.Altitude,
        LocationDescription = d.LocationDescription,
        Notes = d.Notes,
        State = d.State,
        IsOnline = d.IsOnline,
        LastStatusAt = d.LastStatusAt,
        InstalledDate = d.InstalledAt,
        FirmwareVersion = d.FirmwareVersion,
        HardwareVersion = d.HardwareVersion
    };
}
