using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YS.CoffeeMachine.Infrastructure.Migrations.timeDbMigrations
{
    /// <inheritdoc />
    public partial class v121 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlatformOperationLog",
                columns: table => new
                {
                    Timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "(now() AT TIME ZONE 'UTC')"),
                    OperationUserId = table.Column<long>(type: "bigint", nullable: false),
                    OperationUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    TrailType = table.Column<int>(type: "integer", nullable: false),
                    Describe = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Result = table.Column<bool>(type: "boolean", nullable: false),
                    Ip = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    DeviceId = table.Column<long>(type: "bigint", nullable: false),
                    Mid = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EnterpriseinfoId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformOperationLog", x => new { x.OperationUserId, x.Timestamp });
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlatformOperationLog_OperationUserId_Timestamp",
                table: "PlatformOperationLog",
                columns: new[] { "OperationUserId", "Timestamp" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlatformOperationLog");
        }
    }
}
