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
            migrationBuilder.DropIndex(
                name: "IX_Dictionary_IsEnabled",
                table: "Dictionary");

            migrationBuilder.DropIndex(
                name: "IX_Dictionary_Value",
                table: "Dictionary");

            migrationBuilder.AlterColumn<string>(
                name: "IsEnabled",
                table: "Dictionary",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "IsEnabled",
                table: "Dictionary",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.CreateIndex(
                name: "IX_Dictionary_IsEnabled",
                table: "Dictionary",
                column: "IsEnabled");

            migrationBuilder.CreateIndex(
                name: "IX_Dictionary_Value",
                table: "Dictionary",
                column: "Value",
                unique: true);
        }
    }
}
