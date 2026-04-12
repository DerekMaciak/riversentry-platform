using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiverSentry.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFamilyWebsiteAndLogo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                table: "Families",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WebsiteUrl",
                table: "Families",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Families",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111101"),
                columns: new[] { "LogoUrl", "WebsiteUrl" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Families",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111102"),
                columns: new[] { "LogoUrl", "WebsiteUrl" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Families",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111103"),
                columns: new[] { "LogoUrl", "WebsiteUrl" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Families",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111104"),
                columns: new[] { "LogoUrl", "WebsiteUrl" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Families",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111105"),
                columns: new[] { "LogoUrl", "WebsiteUrl" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Families",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111106"),
                columns: new[] { "LogoUrl", "WebsiteUrl" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Families",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111107"),
                columns: new[] { "LogoUrl", "WebsiteUrl" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Families",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111108"),
                columns: new[] { "LogoUrl", "WebsiteUrl" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Families",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111109"),
                columns: new[] { "LogoUrl", "WebsiteUrl" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Families",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111110"),
                columns: new[] { "LogoUrl", "WebsiteUrl" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Families",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "LogoUrl", "WebsiteUrl" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Families",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111112"),
                columns: new[] { "LogoUrl", "WebsiteUrl" },
                values: new object[] { null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoUrl",
                table: "Families");

            migrationBuilder.DropColumn(
                name: "WebsiteUrl",
                table: "Families");
        }
    }
}
