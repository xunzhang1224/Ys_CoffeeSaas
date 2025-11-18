using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YS.CoffeeMachine.Infrastructure.Migrations.timeDbMigrations
{
    /// <inheritdoc />
    public partial class v120 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "devicemetricslog",
                columns: table => new
                {
                    DeviceId = table.Column<long>(type: "bigint", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "(now() AT TIME ZONE 'UTC')"),
                    CounterNo = table.Column<int>(type: "integer", nullable: false),
                    MetricType = table.Column<int>(type: "integer", nullable: false),
                    DeviceName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Index = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Mid = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EnterpriseinfoId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_devicemetricslog", x => new { x.DeviceId, x.Timestamp });
                });

            migrationBuilder.CreateTable(
                name: "deviceonlinelog",
                columns: table => new
                {
                    DeviceId = table.Column<long>(type: "bigint", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "(now() AT TIME ZONE 'UTC')"),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    DeviceName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    DeviceModelName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Mid = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EnterpriseinfoId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deviceonlinelog", x => new { x.DeviceId, x.Timestamp });
                });

            migrationBuilder.CreateIndex(
                name: "IX_devicemetricslog_DeviceId_Timestamp",
                table: "devicemetricslog",
                columns: new[] { "DeviceId", "Timestamp" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_deviceonlinelog_DeviceId_Timestamp",
                table: "deviceonlinelog",
                columns: new[] { "DeviceId", "Timestamp" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "devicemetricslog");

            migrationBuilder.DropTable(
                name: "deviceonlinelog");
        }
    }
}
