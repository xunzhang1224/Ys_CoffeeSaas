using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YS.CoffeeMachine.Infrastructure.Migrations.sqlserverMigrations
{
    /// <inheritdoc />
    public partial class v109 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "P_BeverageInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    DiscountedPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    BeverageIcon = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Temperature = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ProductionForecast = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ForecastQuantity = table.Column<double>(type: "float", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    DisplayStatus = table.Column<bool>(type: "bit", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_P_BeverageInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "P_BeverageVersion",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    BeverageInfoId = table.Column<long>(type: "bigint", nullable: false),
                    VersionType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    BeverageInfoDataString = table.Column<string>(type: "NVARCHAR(MAX)", maxLength: 256, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_P_BeverageVersion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_P_BeverageVersion_P_BeverageInfo_BeverageInfoId",
                        column: x => x.BeverageInfoId,
                        principalTable: "P_BeverageInfo",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "P_FormulaInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    BeverageInfoId = table.Column<long>(type: "bigint", nullable: false),
                    MaterialBoxId = table.Column<long>(type: "bigint", nullable: true),
                    MaterialBoxName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    FormulaType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    SpecsString = table.Column<string>(type: "nvarchar(max)", maxLength: 256, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifyUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_P_FormulaInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_P_FormulaInfo_MaterialBox_MaterialBoxId",
                        column: x => x.MaterialBoxId,
                        principalTable: "MaterialBox",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_P_FormulaInfo_P_BeverageInfo_BeverageInfoId",
                        column: x => x.BeverageInfoId,
                        principalTable: "P_BeverageInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_P_BeverageVersion_BeverageInfoId",
                table: "P_BeverageVersion",
                column: "BeverageInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_P_FormulaInfo_BeverageInfoId",
                table: "P_FormulaInfo",
                column: "BeverageInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_P_FormulaInfo_MaterialBoxId",
                table: "P_FormulaInfo",
                column: "MaterialBoxId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "P_BeverageVersion");

            migrationBuilder.DropTable(
                name: "P_FormulaInfo");

            migrationBuilder.DropTable(
                name: "P_BeverageInfo");
        }
    }
}
