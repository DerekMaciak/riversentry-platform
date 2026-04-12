using Microsoft.EntityFrameworkCore;
using RiverSentry.Domain.Entities;

namespace RiverSentry.Infrastructure.Data;

public class RiverSentryDbContext : DbContext
{
    public RiverSentryDbContext(DbContextOptions<RiverSentryDbContext> options) : base(options) { }

    public DbSet<ProductType> ProductTypes => Set<ProductType>();
    public DbSet<Family> Families => Set<Family>();
    public DbSet<Device> Devices => Set<Device>();
    public DbSet<DeviceStatus> DeviceStatuses => Set<DeviceStatus>();
    public DbSet<MaintenanceRecord> MaintenanceRecords => Set<MaintenanceRecord>();
    public DbSet<AlertEvent> AlertEvents => Set<AlertEvent>();
    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<DeviceRegistration> DeviceRegistrations => Set<DeviceRegistration>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ProductType
        modelBuilder.Entity<ProductType>(e =>
        {
            e.HasKey(p => p.Id);
            e.HasIndex(p => p.Code).IsUnique();
            e.Property(p => p.Code).HasMaxLength(20).IsRequired();
            e.Property(p => p.Name).HasMaxLength(100).IsRequired();
            e.Property(p => p.Description).HasMaxLength(500);
        });

        // Family
        modelBuilder.Entity<Family>(e =>
        {
            e.HasKey(f => f.Id);
            e.Property(f => f.Name).HasMaxLength(200).IsRequired();
            e.Property(f => f.Description).HasMaxLength(1000);
            e.Property(f => f.ContactEmail).HasMaxLength(256);
            e.Property(f => f.ContactPhone).HasMaxLength(50);
            e.Property(f => f.Address).HasMaxLength(500);
        });

        // Device
        modelBuilder.Entity<Device>(e =>
        {
            e.HasKey(d => d.Id);
            e.HasIndex(d => d.MacAddress).IsUnique();

            // Basic Info
            e.Property(d => d.Name).HasMaxLength(200).IsRequired();
            e.Property(d => d.MacAddress).HasMaxLength(12).IsRequired();
            e.Property(d => d.Notes).HasMaxLength(2000);

            // Location & Calibration
            e.Property(d => d.LocationDescription).HasMaxLength(500);

            // Owner Information
            e.Property(d => d.OwnerName).HasMaxLength(200);
            e.Property(d => d.OwnerPhone).HasMaxLength(50);
            e.Property(d => d.OwnerEmail).HasMaxLength(256);
            e.Property(d => d.OwnerAddress1).HasMaxLength(200);
            e.Property(d => d.OwnerAddress2).HasMaxLength(200);
            e.Property(d => d.OwnerCity).HasMaxLength(100);
            e.Property(d => d.OwnerState).HasMaxLength(50);
            e.Property(d => d.OwnerZip).HasMaxLength(20);

            // WiFi Configuration
            e.Property(d => d.WifiSsid).HasMaxLength(64);
            e.Property(d => d.WifiIpAddress).HasMaxLength(45); // IPv6 max length

            // Device Status & Authentication
            e.Property(d => d.ApiKeyHash).HasMaxLength(128);
            e.Property(d => d.FirmwareVersion).HasMaxLength(50);
            e.Property(d => d.HardwareVersion).HasMaxLength(50);

            // Ignore computed properties
            e.Ignore(d => d.IsAlarming);
            e.Ignore(d => d.WifiSignalBars);

            // Relationships
            e.HasOne(d => d.ProductType)
                .WithMany(p => p.Devices)
                .HasForeignKey(d => d.ProductTypeId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(d => d.Family)
                .WithMany(f => f.Devices)
                .HasForeignKey(d => d.FamilyId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // DeviceStatus
        modelBuilder.Entity<DeviceStatus>(e =>
        {
            e.HasKey(s => s.Id);
            e.HasIndex(s => new { s.DeviceId, s.ReceivedAt });
            e.HasOne(s => s.Device)
                .WithMany(d => d.StatusHistory)
                .HasForeignKey(s => s.DeviceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // MaintenanceRecord
        modelBuilder.Entity<MaintenanceRecord>(e =>
        {
            e.HasKey(m => m.Id);
            e.HasIndex(m => new { m.DeviceId, m.PerformedAt });
            e.HasIndex(m => m.ServiceType);
            e.Property(m => m.PerformedBy).HasMaxLength(200);
            e.Property(m => m.Notes).HasMaxLength(2000);
            e.Property(m => m.FirmwareVersionBefore).HasMaxLength(50);
            e.Property(m => m.FirmwareVersionAfter).HasMaxLength(50);
            e.HasOne(m => m.Device)
                .WithMany(d => d.MaintenanceHistory)
                .HasForeignKey(m => m.DeviceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // AlertEvent
        modelBuilder.Entity<AlertEvent>(e =>
        {
            e.HasKey(a => a.Id);
            e.HasIndex(a => a.TriggeredAt);
            e.HasIndex(a => new { a.DeviceId, a.TriggeredAt });
            e.Property(a => a.Description).HasMaxLength(1000);
            e.HasOne(a => a.Device)
                .WithMany(d => d.Alerts)
                .HasForeignKey(a => a.DeviceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // AppUser
        modelBuilder.Entity<AppUser>(e =>
        {
            e.HasKey(u => u.Id);
            e.HasIndex(u => u.ExternalId).IsUnique();
            e.HasIndex(u => u.Email).IsUnique();
            e.Property(u => u.ExternalId).HasMaxLength(128).IsRequired();
            e.Property(u => u.Email).HasMaxLength(256).IsRequired();
            e.Property(u => u.DisplayName).HasMaxLength(200);
        });

        // Subscription
        modelBuilder.Entity<Subscription>(e =>
        {
            e.HasKey(s => s.Id);
            e.HasIndex(s => new { s.UserId, s.DeviceId }).IsUnique();
            e.HasOne(s => s.User)
                .WithMany(u => u.Subscriptions)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(s => s.Device)
                .WithMany(d => d.Subscriptions)
                .HasForeignKey(s => s.DeviceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // DeviceRegistration (for push notifications)
        modelBuilder.Entity<DeviceRegistration>(e =>
        {
            e.HasKey(d => d.Id);
            e.HasIndex(d => d.PushHandle).IsUnique();
            e.Property(d => d.PushHandle).HasMaxLength(512).IsRequired();
            e.HasOne(d => d.User)
                .WithMany(u => u.DeviceRegistrations)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Seed data
        modelBuilder.Entity<ProductType>().HasData(SeedData.GetProductTypes());
        modelBuilder.Entity<Family>().HasData(SeedData.GetFamilies());
    }

    /// <summary>
    /// Seeds devices after database is created (must be called after SaveChanges for ProductTypes/Families).
    /// Also updates existing devices with seed data changes (like install dates).
    /// </summary>
    public async Task SeedDevicesAsync()
    {
        var seedDevices = SeedData.GetDevices();

        if (!await Devices.AnyAsync())
        {
            // Fresh database - add all devices
            Devices.AddRange(seedDevices);
            await SaveChangesAsync();
        }
        else
        {
            // Existing database - update install dates and location descriptions from seed data
            var existingDevices = await Devices.ToListAsync();
            var seedLookup = seedDevices.ToDictionary(d => d.Id);

            foreach (var device in existingDevices)
            {
                if (seedLookup.TryGetValue(device.Id, out var seedDevice))
                {
                    // Update fields that may have changed in seed data
                    if (device.InstalledAt != seedDevice.InstalledAt)
                        device.InstalledAt = seedDevice.InstalledAt;

                    if (device.LocationDescription != seedDevice.LocationDescription)
                        device.LocationDescription = seedDevice.LocationDescription;
                }
            }

            await SaveChangesAsync();
        }
    }
}
