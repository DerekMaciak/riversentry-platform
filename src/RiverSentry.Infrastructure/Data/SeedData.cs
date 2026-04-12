using RiverSentry.Domain.Entities;
using RiverSentry.Domain.Enums;

namespace RiverSentry.Infrastructure.Data;

public static class SeedData
{
    public static ProductType[] GetProductTypes() =>
    [
        new() { Id = 1, Code = "RS-1A", Name = "River Sentry 1A", Description = "Standard flood warning unit", IsActive = true, DisplayOrder = 1 },
        new() { Id = 2, Code = "RS-1B", Name = "River Sentry 1B", Description = "Enhanced unit with upstream relay", IsActive = true, DisplayOrder = 2 },
    ];

    private static readonly DateTime FamilyCreatedDate = new(2024, 2, 1, 0, 0, 0, DateTimeKind.Utc);

    public static Family[] GetFamilies() =>
    [
        new() { Id = Guid.Parse("11111111-1111-1111-1111-111111111101"), Name = "Camp Mystic Guadalupe", Description = "Main campus on the Guadalupe River", IsActive = true, CreatedAt = FamilyCreatedDate },
        new() { Id = Guid.Parse("11111111-1111-1111-1111-111111111102"), Name = "River Run", Description = "River Run location", IsActive = true, CreatedAt = FamilyCreatedDate },
        new() { Id = Guid.Parse("11111111-1111-1111-1111-111111111103"), Name = "Howdy's Bar and Chill", Description = "Howdy's Bar and Chill location", IsActive = true, CreatedAt = FamilyCreatedDate },
        new() { Id = Guid.Parse("11111111-1111-1111-1111-111111111104"), Name = "Camp Kickapoo", Description = "Camp Kickapoo on the Guadalupe", IsActive = true, CreatedAt = FamilyCreatedDate },
        new() { Id = Guid.Parse("11111111-1111-1111-1111-111111111105"), Name = "Camp Mystic Cypress Lake", Description = "Camp Mystic Cypress Lake location", IsActive = true, CreatedAt = FamilyCreatedDate },
        new() { Id = Guid.Parse("11111111-1111-1111-1111-111111111106"), Name = "Camp Chrysalis", Description = "Camp Chrysalis location", IsActive = true, CreatedAt = FamilyCreatedDate },
        new() { Id = Guid.Parse("11111111-1111-1111-1111-111111111107"), Name = "Mo Ranch", Description = "Mo Ranch on the Guadalupe", IsActive = true, CreatedAt = FamilyCreatedDate },
        new() { Id = Guid.Parse("11111111-1111-1111-1111-111111111108"), Name = "Bear Creek Scout Ranch", Description = "Bear Creek Scout Ranch", IsActive = true, CreatedAt = FamilyCreatedDate },
        new() { Id = Guid.Parse("11111111-1111-1111-1111-111111111109"), Name = "Camp Waldemar", Description = "Camp Waldemar on the Guadalupe", IsActive = true, CreatedAt = FamilyCreatedDate },
        new() { Id = Guid.Parse("11111111-1111-1111-1111-111111111110"), Name = "Heart O' The Hills", Description = "Heart O' The Hills camp", IsActive = true, CreatedAt = FamilyCreatedDate },
        new() { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Camp Stewart", Description = "Camp Stewart for Boys", IsActive = true, CreatedAt = FamilyCreatedDate },
        new() { Id = Guid.Parse("11111111-1111-1111-1111-111111111112"), Name = "Camp Honey Creek", Description = "Camp Honey Creek location", IsActive = true, CreatedAt = FamilyCreatedDate },
    ];

    // Note: Seed data is applied via DbContext.OnModelCreating HasData()
    // All values MUST be static/deterministic - no DateTime.Now, Guid.NewGuid(), Random, etc.
    // Base install date for most devices
    private static readonly DateTime InstallDate = new(2024, 3, 15, 10, 0, 0, DateTimeKind.Utc);
    private static readonly DateTime CreatedDate = new(2024, 3, 1, 0, 0, 0, DateTimeKind.Utc);

    // Recent install dates (within last 90 days relative to April 2026)
    private static readonly DateTime RecentInstall1 = new(2026, 3, 28, 10, 0, 0, DateTimeKind.Utc);  // ~8 days ago
    private static readonly DateTime RecentInstall2 = new(2026, 3, 15, 10, 0, 0, DateTimeKind.Utc);  // ~21 days ago
    private static readonly DateTime RecentInstall3 = new(2026, 2, 25, 10, 0, 0, DateTimeKind.Utc);  // ~39 days ago
    private static readonly DateTime RecentInstall4 = new(2026, 2, 10, 10, 0, 0, DateTimeKind.Utc);  // ~54 days ago
    private static readonly DateTime RecentInstall5 = new(2026, 1, 20, 10, 0, 0, DateTimeKind.Utc);  // ~75 days ago

    public static Device[] GetDevices()
    {
        var families = GetFamilies();
        return
        [
            // Camp Mystic Guadalupe - 27 devices (some with recent installs)
            CreateDevice("deadbeef-0000-0000-0001-000000000001", "TXGUACMGRS1A01A", "000101AABBCC", families[0].Id, -99.36950391, 30.00998282, 805.232, RecentInstall1, "Main Bridge Sensor"),
            CreateDevice("deadbeef-0000-0000-0001-000000000002", "TXGUACMGRS1A02A", "000102AABBCC", families[0].Id, -99.36944025, 30.00899141, 817.758, RecentInstall2, "Dining Hall Creek"),
            CreateDevice("deadbeef-0000-0000-0001-000000000003", "TXGUACMGRS1A03A", "000103AABBCC", families[0].Id, -99.3696139, 30.0088627, 818.292),
            CreateDevice("deadbeef-0000-0000-0001-000000000004", "TXGUACMGRS1A04A", "000104AABBCC", families[0].Id, -99.36920505, 30.00913182, 817.085),
            CreateDevice("deadbeef-0000-0000-0001-000000000005", "TXGUACMGRS1A05A", "000105AABBCC", families[0].Id, -99.36766592, 30.00991937, 805.358),
            CreateDevice("deadbeef-0000-0000-0001-000000000006", "TXGUACMGRS1A06A", "000106AABBCC", families[0].Id, -99.37034405, 30.00968917, 807.596),
            CreateDevice("deadbeef-0000-0000-0001-000000000007", "TXGUACMGRS1A07A", "000107AABBCC", families[0].Id, -99.37063273, 30.00960792, 806.325),
            CreateDevice("deadbeef-0000-0000-0001-000000000008", "TXGUACMGRS1A08A", "000108AABBCC", families[0].Id, -99.37101099, 30.00944751, 808.124),
            CreateDevice("deadbeef-0000-0000-0001-000000000009", "TXGUACMGRS1A09A", "000109AABBCC", families[0].Id, -99.37160044, 30.00912241, 811.231),
            CreateDevice("deadbeef-0000-0000-0001-000000000010", "TXGUACMGRS1A10A", "00010AAABBCC", families[0].Id, -99.37197293, 30.00978870, 819.902),
            CreateDevice("deadbeef-0000-0000-0001-000000000011", "TXGUACMGRS1A11A", "00010BAABBCC", families[0].Id, -99.37216845, 30.00985960, 825.298),
            CreateDevice("deadbeef-0000-0000-0001-000000000012", "TXGUACMGRS1A12A", "00010CAABBCC", families[0].Id, -99.37238914, 30.00997181, 826.696),
            CreateDevice("deadbeef-0000-0000-0001-000000000013", "TXGUACMGRS1A13A", "00010DAABBCC", families[0].Id, -99.37238909, 30.00997181, 826.691),
            CreateDevice("deadbeef-0000-0000-0001-000000000014", "TXGUACMGRS1A14A", "00010EAABBCC", families[0].Id, -99.37242419, 30.00934914, 826.945),
            CreateDevice("deadbeef-0000-0000-0001-000000000015", "TXGUACMGRS1A15A", "00010FAABBCC", families[0].Id, -99.37199326, 30.00958637, 825.074),
            CreateDevice("deadbeef-0000-0000-0001-000000000016", "TXGUACMGRS1A16A", "000110AABBCC", families[0].Id, -99.37240688, 30.00863478, 816.201),
            CreateDevice("deadbeef-0000-0000-0001-000000000017", "TXGUACMGRS1A17A", "000111AABBCC", families[0].Id, -99.37280102, 30.00830106, 817.741),
            CreateDevice("deadbeef-0000-0000-0001-000000000018", "TXGUACMGRS1A18A", "000112AABBCC", families[0].Id, -99.37127382, 30.00809114, 815.109),
            CreateDevice("deadbeef-0000-0000-0001-000000000019", "TXGUACMGRS1A19A", "000113AABBCC", families[0].Id, -99.37110596, 30.00778961, 817.479),
            CreateDevice("deadbeef-0000-0000-0001-000000000020", "TXGUACMGRS1A20A", "000114AABBCC", families[0].Id, -99.37064880, 30.00758358, 823.065),
            CreateDevice("deadbeef-0000-0000-0001-000000000021", "TXGUACMGRS1A21A", "000115AABBCC", families[0].Id, -99.37062795, 30.00758712, 824.851),
            CreateDevice("deadbeef-0000-0000-0001-000000000022", "TXGUACMGRS1A22A", "000116AABBCC", families[0].Id, -99.37068589, 30.00759065, 785.946),
            CreateDevice("deadbeef-0000-0000-0001-000000000023", "TXGUACMGRS1A23A", "000117AABBCC", families[0].Id, -99.36753384, 30.00736011, 788.788),
            CreateDevice("deadbeef-0000-0000-0001-000000000024", "TXGUACMGRS1A24A", "000118AABBCC", families[0].Id, -99.37494980, 30.00654478, 770.608),
            CreateDevice("deadbeef-0000-0000-0001-000000000025", "TXGUACMGRS1A25A", "000119AABBCC", families[0].Id, -99.36806335, 30.00703359, 787.874),
            CreateDevice("deadbeef-0000-0000-0001-000000000026", "TXGUACMGRS1A26A", "00011AAABBCC", families[0].Id, -99.37417550, 30.01173905, 808.348),
            CreateDevice("deadbeef-0000-0000-0001-000000000027", "TXGUACMGRS1A27A", "00011BAABBCC", families[0].Id, -99.36782822, 30.01103702, 806.834),

            // River Run - 2 devices
            CreateDevice("deadbeef-0000-0000-0002-000000000001", "TXGUARRRS1A01A", "000201AABBCC", families[1].Id, -99.22289263, 30.06798887, 606.492),
            CreateDevice("deadbeef-0000-0000-0002-000000000002", "TXGUARRRS1A02A", "000202AABBCC", families[1].Id, -99.22315917, 30.06812038, 608.269),

            // Howdy's Bar and Chill - 4 devices
            CreateDevice("deadbeef-0000-0000-0003-000000000001", "TXGUAHBCRS1A01A", "000301AABBCC", families[2].Id, -99.22468281, 30.06877714, 606.305),
            CreateDevice("deadbeef-0000-0000-0003-000000000002", "TXGUAHBCRS1A02A", "000302AABBCC", families[2].Id, -99.22431200, 30.06863946, 613.944),
            CreateDevice("deadbeef-0000-0000-0003-000000000003", "TXGUAHBCRS1A03A", "000303AABBCC", families[2].Id, -99.22398428, 30.06863208, 617.401),
            CreateDevice("deadbeef-0000-0000-0003-000000000004", "TXGUAHBCRS1A04A", "000304AABBCC", families[2].Id, -99.22379703, 30.06843206, 614.417),

            // Camp Kickapoo - 3 devices
            CreateDevice("deadbeef-0000-0000-0004-000000000001", "TXGUACKPRS1A01A", "000401AABBCC", families[3].Id, -99.22319233, 29.97296512, 670.102),
            CreateDevice("deadbeef-0000-0000-0004-000000000002", "TXGUACKPRS1A02A", "000402AABBCC", families[3].Id, -99.22467612, 29.97454802, 679.047),
            CreateDevice("deadbeef-0000-0000-0004-000000000003", "TXGUACKPRS1A03A", "000403AABBCC", families[3].Id, -99.22113529, 29.97221406, 663.961),

            // Camp Mystic Cypress Lake - 4 devices
            CreateDevice("deadbeef-0000-0000-0005-000000000001", "TXGUACMCLRS1A01A", "000501AABBCC", families[4].Id, -99.37066709, 30.00400325, 507.7),
            CreateDevice("deadbeef-0000-0000-0005-000000000002", "TXGUACMCLRS1A02A", "000502AABBCC", families[4].Id, -99.37138455, 30.00400639, 508.308),
            CreateDevice("deadbeef-0000-0000-0005-000000000003", "TXGUACMCLRS1A03A", "000503AABBCC", families[4].Id, -99.37336522, 30.00410015, 507.713),
            CreateDevice("deadbeef-0000-0000-0005-000000000004", "TXGUACMCLRS1A04A", "000504AABBCC", families[4].Id, -99.37494737, 30.00653606, 540.973),

            // Camp Chrysalis - 4 devices
            CreateDevice("deadbeef-0000-0000-0006-000000000001", "TXGUACCRS1A01A", "000601AABBCC", families[5].Id, -99.22241676, 29.98097138, 518.611),
            CreateDevice("deadbeef-0000-0000-0006-000000000002", "TXGUACCRS1A02A", "000602AABBCC", families[5].Id, -99.22275495, 29.98061086, 519.061),
            CreateDevice("deadbeef-0000-0000-0006-000000000003", "TXGUACCRS1A03A", "000603AABBCC", families[5].Id, -99.22301886, 29.98038644, 519.196),
            CreateDevice("deadbeef-0000-0000-0006-000000000004", "TXGUACCRS1A04A", "000604AABBCC", families[5].Id, -99.22373977, 29.97992953, 520.0),

            // Mo Ranch - 6 devices (1 recent)
            CreateDevice("deadbeef-0000-0000-0007-000000000001", "TXGUAMRRS1A01A", "000701AABBCC", families[6].Id, -99.46961611, 30.06207745, 831.912, RecentInstall3, "River Crossing North"),
            CreateDevice("deadbeef-0000-0000-0007-000000000002", "TXGUAMRRS1A02A", "000702AABBCC", families[6].Id, -99.46994964, 30.06178873, 833.121),
            CreateDevice("deadbeef-0000-0000-0007-000000000003", "TXGUAMRRS1A03A", "000703AABBCC", families[6].Id, -99.47075460, 30.05989571, 840.439),
            CreateDevice("deadbeef-0000-0000-0007-000000000004", "TXGUAMRRS1A04A", "000704AABBCC", families[6].Id, -99.47083537, 30.05967711, 841.147),
            CreateDevice("deadbeef-0000-0000-0007-000000000005", "TXGUAMRRS1A05A", "000705AABBCC", families[6].Id, -99.47089839, 30.05952082, 842.536),
            CreateDevice("deadbeef-0000-0000-0007-000000000006", "TXGUAMRRS1A06A", "000706AABBCC", families[6].Id, -99.47102268, 30.05926831, 843.584),

            // Bear Creek Scout Ranch - 7 devices (2 recent)
            CreateDevice("deadbeef-0000-0000-0008-000000000001", "TXGUABCSRRS1A01A", "000801AABBCC", families[7].Id, -99.42522693, 30.06640972, 813.977, RecentInstall4, "Lodge Creek Sensor"),
            CreateDevice("deadbeef-0000-0000-0008-000000000002", "TXGUABCSRRS1A02A", "000802AABBCC", families[7].Id, -99.42549197, 30.06682625, 814.555, RecentInstall5, "Campsite Delta"),
            CreateDevice("deadbeef-0000-0000-0008-000000000003", "TXGUABCSRRS1A03A", "000803AABBCC", families[7].Id, -99.42663694, 30.06875348, 822.175),
            CreateDevice("deadbeef-0000-0000-0008-000000000004", "TXGUABCSRRS1A04A", "000804AABBCC", families[7].Id, -99.42777631, 30.07001801, 825.36),
            CreateDevice("deadbeef-0000-0000-0008-000000000005", "TXGUABCSRRS1A05A", "000805AABBCC", families[7].Id, -99.42845193, 30.07095002, 818.353),
            CreateDevice("deadbeef-0000-0000-0008-000000000006", "TXGUABCSRRS1A06A", "000806AABBCC", families[7].Id, -99.43195914, 30.07122278, 835.326),
            CreateDevice("deadbeef-0000-0000-0008-000000000007", "TXGUABCSRRS1A07A", "000807AABBCC", families[7].Id, -99.43123548, 30.07103427, 836.601),

            // Camp Waldemar - 2 devices
            CreateDevice("deadbeef-0000-0000-0009-000000000001", "TXGUACWRS1A01A", "000901AABBCC", families[8].Id, -99.39774141, 30.06171351, 770.805),
            CreateDevice("deadbeef-0000-0000-0009-000000000002", "TXGUACWRS1A02A", "000902AABBCC", families[8].Id, -99.39979167, 30.06199463, 778.13),

            // Heart O' The Hills - 2 devices
            CreateDevice("deadbeef-0000-0000-000a-000000000001", "TXGUAHOHRS1A01A", "000A01AABBCC", families[9].Id, -99.37292199, 30.06480066, 756.54),
            CreateDevice("deadbeef-0000-0000-000a-000000000002", "TXGUAHOHRS1A02A", "000A02AABBCC", families[9].Id, -99.37384396, 30.06546065, 756.651),

            // Camp Stewart - 6 devices
            CreateDevice("deadbeef-0000-0000-000b-000000000001", "TXGUACSRS1A01A", "000B01AABBCC", families[10].Id, -99.36545310, 30.06465842, 776.611),
            CreateDevice("deadbeef-0000-0000-000b-000000000002", "TXGUACSRS1A02A", "000B02AABBCC", families[10].Id, -99.36583385, 30.06436170, 776.779),
            CreateDevice("deadbeef-0000-0000-000b-000000000003", "TXGUACSRS1A03A", "000B03AABBCC", families[10].Id, -99.36625949, 30.06406551, 777.063),
            CreateDevice("deadbeef-0000-0000-000b-000000000004", "TXGUACSRS1A04A", "000B04AABBCC", families[10].Id, -99.36680324, 30.06381940, 777.436),
            CreateDevice("deadbeef-0000-0000-000b-000000000005", "TXGUACSRS1A05A", "000B05AABBCC", families[10].Id, -99.36736492, 30.06374380, 778.278),
            CreateDevice("deadbeef-0000-0000-000b-000000000006", "TXGUACSRS1A06A", "000B06AABBCC", families[10].Id, -99.36643365, 30.06098017, 775.886),

            // Camp Honey Creek - 11 devices
            CreateDevice("deadbeef-0000-0000-000c-000000000001", "TXGUACHCRS1A01A", "000C01AABBCC", families[11].Id, -99.35371802, 30.09269175, 783.04),
            CreateDevice("deadbeef-0000-0000-000c-000000000002", "TXGUACHCRS1A02A", "000C02AABBCC", families[11].Id, -99.35365375, 30.09291723, 783.491),
            CreateDevice("deadbeef-0000-0000-000c-000000000003", "TXGUACHCRS1A03A", "000C03AABBCC", families[11].Id, -99.35348924, 30.09322647, 782.967),
            CreateDevice("deadbeef-0000-0000-000c-000000000004", "TXGUACHCRS1A04A", "000C04AABBCC", families[11].Id, -99.34966334, 30.09449588, 807.945),
            CreateDevice("deadbeef-0000-0000-000c-000000000005", "TXGUACHCRS1A05A", "000C05AABBCC", families[11].Id, -99.34958643, 30.09438655, 804.801),
            CreateDevice("deadbeef-0000-0000-000c-000000000006", "TXGUACHCRS1A06A", "000C06AABBCC", families[11].Id, -99.34954132, 30.09424209, 802.293),
            CreateDevice("deadbeef-0000-0000-000c-000000000007", "TXGUACHCRS1A07A", "000C07AABBCC", families[11].Id, -99.35016923, 30.09496923, 816.025),
            CreateDevice("deadbeef-0000-0000-000c-000000000008", "TXGUACHCRS1A08A", "000C08AABBCC", families[11].Id, -99.35041433, 30.09505477, 819.107),
            CreateDevice("deadbeef-0000-0000-000c-000000000009", "TXGUACHCRS1A09A", "000C09AABBCC", families[11].Id, -99.35066329, 30.09511713, 819.38),
            CreateDevice("deadbeef-0000-0000-000c-000000000010", "TXGUACHCRS1A10A", "000C0AAABBCC", families[11].Id, -99.35137778, 30.09494761, 807.749),
            CreateDevice("deadbeef-0000-0000-000c-000000000011", "TXGUACHCRS1A11A", "000C0BAABBCC", families[11].Id, -99.35157246, 30.09497971, 809.67),
        ];
    }

    private static Device CreateDevice(string id, string name, string mac, Guid familyId, double lon, double lat, double elevation, DateTime? installedAt = null, string? locationDesc = null) => new()
    {
        Id = Guid.Parse(id),
        Name = name,
        MacAddress = mac,
        ProductTypeId = 1, // RS-1A
        FamilyId = familyId,
        Longitude = lon,
        Latitude = lat,
        Altitude = elevation * 0.3048, // Convert feet to meters
        Elevation = elevation,
        WaterElevation = elevation - 15,
        HeightAboveWater = 15,
        LocationDescription = locationDesc,
        State = DeviceState.Armed,
        IsOnline = true,
        FirmwareVersion = "1.0.0",
        HardwareVersion = "1.0.0",
        InstalledAt = installedAt ?? InstallDate,
        LastServiceAt = installedAt ?? InstallDate,
        CreatedAt = CreatedDate,
    };
}
