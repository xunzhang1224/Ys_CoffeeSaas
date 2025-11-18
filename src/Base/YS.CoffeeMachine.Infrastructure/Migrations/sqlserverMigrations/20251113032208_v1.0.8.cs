using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YS.CoffeeMachine.Infrastructure.Migrations.sqlserverMigrations
{
    /// <inheritdoc />
    public partial class v108 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_P_FormulaInfo_MaterialBox_MaterialBoxId",
                table: "P_FormulaInfo");

            migrationBuilder.DropIndex(
                name: "IX_P_FormulaInfo_MaterialBoxId",
                table: "P_FormulaInfo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_P_FormulaInfo_MaterialBoxId",
                table: "P_FormulaInfo",
                column: "MaterialBoxId");

            migrationBuilder.AddForeignKey(
                name: "FK_P_FormulaInfo_MaterialBox_MaterialBoxId",
                table: "P_FormulaInfo",
                column: "MaterialBoxId",
                principalTable: "MaterialBox",
                principalColumn: "Id");
        }
    }
}
