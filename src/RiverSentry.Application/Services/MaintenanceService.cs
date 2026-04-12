using RiverSentry.Application.Interfaces;
using RiverSentry.Contracts.DTOs;
using RiverSentry.Contracts.Requests;
using RiverSentry.Domain.Entities;
using RiverSentry.Domain.Enums;

namespace RiverSentry.Application.Services;

public class MaintenanceService
{
    private readonly IMaintenanceRepository _maintenanceRepo;
    private readonly IDeviceRepository _deviceRepo;

    public MaintenanceService(IMaintenanceRepository maintenanceRepo, IDeviceRepository deviceRepo)
    {
        _maintenanceRepo = maintenanceRepo;
        _deviceRepo = deviceRepo;
    }

    public async Task<IReadOnlyList<MaintenanceRecordDto>> GetByDeviceAsync(Guid deviceId, CancellationToken ct = default)
    {
        var records = await _maintenanceRepo.GetByDeviceAsync(deviceId, ct);
        var device = await _deviceRepo.GetByIdAsync(deviceId, ct);
        return records.Select(r => MapToDto(r, device?.Name)).ToList();
    }

    public async Task<IReadOnlyList<MaintenanceRecordDto>> GetRecentAsync(int count = 50, CancellationToken ct = default)
    {
        var records = await _maintenanceRepo.GetRecentAsync(count, ct);
        return records.Select(r => MapToDto(r, r.Device?.Name)).ToList();
    }

    public async Task<MaintenanceRecordDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var record = await _maintenanceRepo.GetByIdAsync(id, ct);
        return record is null ? null : MapToDto(record, record.Device?.Name);
    }

    public async Task<MaintenanceRecordDto> CreateAsync(CreateMaintenanceRequest request, CancellationToken ct = default)
    {
        var device = await _deviceRepo.GetByIdAsync(request.DeviceId, ct)
            ?? throw new InvalidOperationException($"Device {request.DeviceId} not found");

        var record = new MaintenanceRecord
        {
            Id = Guid.NewGuid(),
            DeviceId = request.DeviceId,
            ServiceType = request.ServiceType,
            PerformedAt = request.PerformedAt,
            PerformedBy = request.PerformedBy,
            Notes = request.Notes,
            FirmwareVersionBefore = request.FirmwareVersionBefore,
            FirmwareVersionAfter = request.FirmwareVersionAfter,
            NewLatitude = request.NewLatitude,
            NewLongitude = request.NewLongitude,
            CreatedAt = DateTime.UtcNow
        };

        await _maintenanceRepo.AddAsync(record, ct);

        // Update device's LastServiceAt
        device.LastServiceAt = request.PerformedAt;

        // If firmware was updated, update device firmware version
        if (!string.IsNullOrEmpty(request.FirmwareVersionAfter))
        {
            device.FirmwareVersion = request.FirmwareVersionAfter;
        }

        // If device was relocated, update coordinates
        if (request.NewLatitude.HasValue && request.NewLongitude.HasValue)
        {
            device.Latitude = request.NewLatitude.Value;
            device.Longitude = request.NewLongitude.Value;
        }

        await _deviceRepo.UpdateAsync(device, ct);

        return MapToDto(record, device.Name);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        await _maintenanceRepo.DeleteAsync(id, ct);
    }

    private static MaintenanceRecordDto MapToDto(MaintenanceRecord r, string? deviceName = null) => new()
    {
        Id = r.Id,
        DeviceId = r.DeviceId,
        DeviceName = deviceName ?? string.Empty,
        ServiceType = r.ServiceType,
        ServiceTypeName = FormatServiceType(r.ServiceType),
        PerformedAt = r.PerformedAt,
        PerformedBy = r.PerformedBy,
        Notes = r.Notes,
        FirmwareVersionBefore = r.FirmwareVersionBefore,
        FirmwareVersionAfter = r.FirmwareVersionAfter,
        NewLatitude = r.NewLatitude,
        NewLongitude = r.NewLongitude,
        CreatedAt = r.CreatedAt
    };

    private static string FormatServiceType(ServiceType type) => type switch
    {
        ServiceType.Installation => "Installation",
        ServiceType.Inspection => "Inspection",
        ServiceType.Repair => "Repair",
        ServiceType.FirmwareUpdate => "Firmware Update",
        ServiceType.BatteryReplacement => "Battery Replacement",
        ServiceType.SensorCalibration => "Sensor Calibration",
        ServiceType.Relocation => "Relocation",
        ServiceType.Decommission => "Decommission",
        _ => type.ToString()
    };
}
