using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TbspRpgDataLayer.Migrations
{
    public partial class AddAdventureToScript : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_adventures_InitializationScriptId",
                table: "adventures");

            migrationBuilder.DropIndex(
                name: "IX_adventures_TerminationScriptId",
                table: "adventures");

            migrationBuilder.AddColumn<Guid>(
                name: "AdventureId",
                table: "scripts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_scripts_AdventureId",
                table: "scripts",
                column: "AdventureId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_scripts_adventures_AdventureId",
                table: "scripts",
                column: "AdventureId",
                principalTable: "adventures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_scripts_adventures_AdventureId",
                table: "scripts");

            migrationBuilder.DropIndex(
                name: "IX_scripts_AdventureId",
                table: "scripts");

            migrationBuilder.DropIndex(
                name: "IX_adventures_InitializationScriptId",
                table: "adventures");

            migrationBuilder.DropIndex(
                name: "IX_adventures_TerminationScriptId",
                table: "adventures");

            migrationBuilder.DropColumn(
                name: "AdventureId",
                table: "scripts");

            migrationBuilder.CreateIndex(
                name: "IX_adventures_InitializationScriptId",
                table: "adventures",
                column: "InitializationScriptId");

            migrationBuilder.CreateIndex(
                name: "IX_adventures_TerminationScriptId",
                table: "adventures",
                column: "TerminationScriptId");
        }
    }
}
