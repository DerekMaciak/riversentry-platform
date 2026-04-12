using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using RiverSentry.Application.Interfaces;
using RiverSentry.Application.Services;
using RiverSentry.Infrastructure.Data;
using RiverSentry.Infrastructure.Repositories;
using RiverSentry.Infrastructure.Services;

namespace RiverSentry.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        // Database
        services.AddDbContext<RiverSentryDbContext>(options =>
            options.UseSqlServer(connectionString)
                .ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning)));

        // Repositories
        services.AddScoped<IDeviceRepository, DeviceRepository>();
        services.AddScoped<IDeviceStatusRepository, DeviceStatusRepository>();
        services.AddScoped<IAlertRepository, AlertRepository>();
        services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
        services.AddScoped<IMaintenanceRepository, MaintenanceRepository>();
        services.AddScoped<IFamilyRepository, FamilyRepository>();

        // Application services
        services.AddScoped<DeviceService>();
        services.AddScoped<AlertService>();
        services.AddScoped<MaintenanceService>();
        services.AddScoped<FamilyService>();

        // Notification (stub — replace with Azure Notification Hubs in Phase 3)
        services.AddScoped<INotificationDispatcher, StubNotificationDispatcher>();

        return services;
    }
}
