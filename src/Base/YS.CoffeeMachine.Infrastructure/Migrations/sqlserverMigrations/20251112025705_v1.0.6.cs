using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YS.CoffeeMachine.Infrastructure.Migrations.sqlserverMigrations
{
    /// <inheritdoc />
    public partial class v106 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "DeviceInfo",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "District",
                table: "DeviceInfo",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Lat",
                table: "DeviceInfo",
                type: "decimal(18,10)",
                precision: 18,
                scale: 10,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Lng",
                table: "DeviceInfo",
                type: "decimal(18,10)",
                precision: 18,
                scale: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Province",
                table: "DeviceInfo",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "DeviceInfo",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "District",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "Lat",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "Lng",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "Province",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "DeviceInfo");
        }
    }
}
