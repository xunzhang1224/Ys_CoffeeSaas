using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YS.CoffeeMachine.Infrastructure.Migrations.sqlserverMigrations
{
    /// <inheritdoc />
    public partial class v107 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DictionaryEntity_DictionaryEntity_ParentKey",
                table: "DictionaryEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DictionaryEntity",
                table: "DictionaryEntity");

            migrationBuilder.RenameTable(
                name: "DictionaryEntity",
                newName: "Dictionary");

            migrationBuilder.RenameIndex(
                name: "IX_DictionaryEntity_ParentKey",
                table: "Dictionary",
                newName: "IX_Dictionary_ParentKey");

            migrationBuilder.AlterColumn<int>(
                name: "IsEnabled",
                table: "Dictionary",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Dictionary",
                table: "Dictionary",
                column: "Key");

            migrationBuilder.CreateIndex(
                name: "IX_Dictionary_IsEnabled",
                table: "Dictionary",
                column: "IsEnabled");

            migrationBuilder.CreateIndex(
                name: "IX_Dictionary_Value",
                table: "Dictionary",
                column: "Value",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Dictionary_Dictionary_ParentKey",
                table: "Dictionary",
                column: "ParentKey",
                principalTable: "Dictionary",
                principalColumn: "Key",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dictionary_Dictionary_ParentKey",
                table: "Dictionary");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Dictionary",
                table: "Dictionary");

            migrationBuilder.DropIndex(
                name: "IX_Dictionary_IsEnabled",
                table: "Dictionary");

            migrationBuilder.DropIndex(
                name: "IX_Dictionary_Value",
                table: "Dictionary");

            migrationBuilder.RenameTable(
                name: "Dictionary",
                newName: "DictionaryEntity");

            migrationBuilder.RenameIndex(
                name: "IX_Dictionary_ParentKey",
                table: "DictionaryEntity",
                newName: "IX_DictionaryEntity_ParentKey");

            migrationBuilder.AlterColumn<string>(
                name: "IsEnabled",
                table: "DictionaryEntity",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DictionaryEntity",
                table: "DictionaryEntity",
                column: "Key");

            migrationBuilder.AddForeignKey(
                name: "FK_DictionaryEntity_DictionaryEntity_ParentKey",
                table: "DictionaryEntity",
                column: "ParentKey",
                principalTable: "DictionaryEntity",
                principalColumn: "Key",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
