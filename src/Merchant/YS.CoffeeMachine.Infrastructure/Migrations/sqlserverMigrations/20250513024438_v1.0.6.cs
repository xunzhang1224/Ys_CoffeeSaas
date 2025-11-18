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
            migrationBuilder.CreateTable(
                name: "DictionaryEntity",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ParentKey = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsEnabled = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DictionaryEntity", x => x.Key);
                    table.ForeignKey(
                        name: "FK_DictionaryEntity_DictionaryEntity_ParentKey",
                        column: x => x.ParentKey,
                        principalTable: "DictionaryEntity",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DictionaryEntity_ParentKey",
                table: "DictionaryEntity",
                column: "ParentKey");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DictionaryEntity");
        }
    }
}
