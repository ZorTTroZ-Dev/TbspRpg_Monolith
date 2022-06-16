using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TbspRpgDataLayer.Migrations
{
    public partial class ScriptAdventureModelUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_adventures_InitializationScriptId",
                table: "adventures");

            migrationBuilder.DropIndex(
                name: "IX_adventures_TerminationScriptId",
                table: "adventures");

            migrationBuilder.CreateIndex(
                name: "IX_adventures_InitializationScriptId",
                table: "adventures",
                column: "InitializationScriptId");

            migrationBuilder.CreateIndex(
                name: "IX_adventures_TerminationScriptId",
                table: "adventures",
                column: "TerminationScriptId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_adventures_InitializationScriptId",
                table: "adventures");

            migrationBuilder.DropIndex(
                name: "IX_adventures_TerminationScriptId",
                table: "adventures");

            migrationBuilder.CreateIndex(
                name: "IX_adventures_InitializationScriptId",
                table: "adventures",
                column: "InitializationScriptId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_adventures_TerminationScriptId",
                table: "adventures",
                column: "TerminationScriptId",
                unique: true);
        }
    }
}
