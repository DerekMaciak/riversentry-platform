using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RiverSentry.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Families",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ContactEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ContactPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Families", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExternalId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MacAddress = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    ProductTypeId = table.Column<int>(type: "int", nullable: false),
                    FamilyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Altitude = table.Column<double>(type: "float", nullable: true),
                    Elevation = table.Column<double>(type: "float", nullable: true),
                    WaterElevation = table.Column<double>(type: "float", nullable: true),
                    HeightAboveWater = table.Column<double>(type: "float", nullable: true),
                    LocationDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    OwnerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    OwnerPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OwnerEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    OwnerAddress1 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    OwnerAddress2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    OwnerCity = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OwnerState = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OwnerZip = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    WifiSsid = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    WifiConnected = table.Column<bool>(type: "bit", nullable: false),
                    WifiRssi = table.Column<int>(type: "int", nullable: true),
                    WifiIpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    ApiKeyHash = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    IsOnline = table.Column<bool>(type: "bit", nullable: false),
                    FirmwareVersion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HardwareVersion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InstalledAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastServiceAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastStatusAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Devices_Families_FamilyId",
                        column: x => x.FamilyId,
                        principalTable: "Families",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Devices_ProductTypes_ProductTypeId",
                        column: x => x.ProductTypeId,
                        principalTable: "ProductTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DeviceRegistrations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Platform = table.Column<int>(type: "int", nullable: false),
                    PushHandle = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastActiveAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceRegistrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceRegistrations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AlertEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AlarmType = table.Column<int>(type: "int", nullable: false),
                    Severity = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    TriggeredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResolvedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlertEvents_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeviceStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    MainVolts = table.Column<double>(type: "float", nullable: false),
                    BatteryVolts = table.Column<double>(type: "float", nullable: false),
                    WaterWitch1 = table.Column<bool>(type: "bit", nullable: false),
                    WaterWitch2 = table.Column<bool>(type: "bit", nullable: false),
                    WifiConnected = table.Column<bool>(type: "bit", nullable: false),
                    NtpSync = table.Column<bool>(type: "bit", nullable: false),
                    ReceivedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceStatuses_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceType = table.Column<int>(type: "int", nullable: false),
                    PerformedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PerformedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    FirmwareVersionBefore = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FirmwareVersionAfter = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NewLatitude = table.Column<double>(type: "float", nullable: true),
                    NewLongitude = table.Column<double>(type: "float", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceRecords_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AlarmTypes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Families",
                columns: new[] { "Id", "Address", "ContactEmail", "ContactPhone", "CreatedAt", "Description", "IsActive", "Name" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111101"), null, null, null, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Main campus on the Guadalupe River", true, "Camp Mystic Guadalupe" },
                    { new Guid("11111111-1111-1111-1111-111111111102"), null, null, null, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), "River Run location", true, "River Run" },
                    { new Guid("11111111-1111-1111-1111-111111111103"), null, null, null, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Howdy's Bar and Chill location", true, "Howdy's Bar and Chill" },
                    { new Guid("11111111-1111-1111-1111-111111111104"), null, null, null, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Camp Kickapoo on the Guadalupe", true, "Camp Kickapoo" },
                    { new Guid("11111111-1111-1111-1111-111111111105"), null, null, null, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Camp Mystic Cypress Lake location", true, "Camp Mystic Cypress Lake" },
                    { new Guid("11111111-1111-1111-1111-111111111106"), null, null, null, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Camp Chrysalis location", true, "Camp Chrysalis" },
                    { new Guid("11111111-1111-1111-1111-111111111107"), null, null, null, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Mo Ranch on the Guadalupe", true, "Mo Ranch" },
                    { new Guid("11111111-1111-1111-1111-111111111108"), null, null, null, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Bear Creek Scout Ranch", true, "Bear Creek Scout Ranch" },
                    { new Guid("11111111-1111-1111-1111-111111111109"), null, null, null, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Camp Waldemar on the Guadalupe", true, "Camp Waldemar" },
                    { new Guid("11111111-1111-1111-1111-111111111110"), null, null, null, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Heart O' The Hills camp", true, "Heart O' The Hills" },
                    { new Guid("11111111-1111-1111-1111-111111111111"), null, null, null, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Camp Stewart for Boys", true, "Camp Stewart" },
                    { new Guid("11111111-1111-1111-1111-111111111112"), null, null, null, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Camp Honey Creek location", true, "Camp Honey Creek" }
                });

            migrationBuilder.InsertData(
                table: "ProductTypes",
                columns: new[] { "Id", "Code", "Description", "DisplayOrder", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, "RS-1A", "Standard flood warning unit", 1, true, "River Sentry 1A" },
                    { 2, "RS-1B", "Enhanced unit with upstream relay", 2, true, "River Sentry 1B" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlertEvents_DeviceId_TriggeredAt",
                table: "AlertEvents",
                columns: new[] { "DeviceId", "TriggeredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_AlertEvents_TriggeredAt",
                table: "AlertEvents",
                column: "TriggeredAt");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceRegistrations_PushHandle",
                table: "DeviceRegistrations",
                column: "PushHandle",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeviceRegistrations_UserId",
                table: "DeviceRegistrations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_FamilyId",
                table: "Devices",
                column: "FamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_MacAddress",
                table: "Devices",
                column: "MacAddress",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Devices_ProductTypeId",
                table: "Devices",
                column: "ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceStatuses_DeviceId_ReceivedAt",
                table: "DeviceStatuses",
                columns: new[] { "DeviceId", "ReceivedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRecords_DeviceId_PerformedAt",
                table: "MaintenanceRecords",
                columns: new[] { "DeviceId", "PerformedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRecords_ServiceType",
                table: "MaintenanceRecords",
                column: "ServiceType");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTypes_Code",
                table: "ProductTypes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_DeviceId",
                table: "Subscriptions",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_UserId_DeviceId",
                table: "Subscriptions",
                columns: new[] { "UserId", "DeviceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ExternalId",
                table: "Users",
                column: "ExternalId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlertEvents");

            migrationBuilder.DropTable(
                name: "DeviceRegistrations");

            migrationBuilder.DropTable(
                name: "DeviceStatuses");

            migrationBuilder.DropTable(
                name: "MaintenanceRecords");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Families");

            migrationBuilder.DropTable(
                name: "ProductTypes");
        }
    }
}
