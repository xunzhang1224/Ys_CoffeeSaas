using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YS.CoffeeMachine.Infrastructure.Migrations.sqlserverMigrations
{
    /// <inheritdoc />
    public partial class v105 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OperationResult",
                table: "OperationLog",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "SpecsString",
                table: "FormulaInfoTemplate",
                type: "nvarchar(max)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "SpecsString",
                table: "FormulaInfo",
                type: "nvarchar(max)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "MenuIds",
                table: "EnterpriseInfo",
                type: "nvarchar(max)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "BeverageInfoDataString",
                table: "BeverageTemplateVersion",
                type: "nvarchar(max)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "BeverageNames",
                table: "BeverageCollection",
                type: "nvarchar(max)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "BeverageIds",
                table: "BeverageCollection",
                type: "nvarchar(max)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldMaxLength: 256);

            migrationBuilder.AddColumn<string>(
                name: "ActivePath",
                table: "ApplicationMenu",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OperationResult",
                table: "OperationLog");

            migrationBuilder.DropColumn(
                name: "ActivePath",
                table: "ApplicationMenu");

            migrationBuilder.AlterColumn<string>(
                name: "SpecsString",
                table: "FormulaInfoTemplate",
                type: "text",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "SpecsString",
                table: "FormulaInfo",
                type: "text",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "MenuIds",
                table: "EnterpriseInfo",
                type: "text",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "BeverageInfoDataString",
                table: "BeverageTemplateVersion",
                type: "text",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "BeverageNames",
                table: "BeverageCollection",
                type: "text",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "BeverageIds",
                table: "BeverageCollection",
                type: "text",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 256);
        }
    }
}
