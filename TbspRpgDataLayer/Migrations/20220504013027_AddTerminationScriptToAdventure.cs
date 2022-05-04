using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TbspRpgDataLayer.Migrations
{
    public partial class AddTerminationScriptToAdventure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TerminationScriptId",
                table: "adventures",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_adventures_TerminationScriptId",
                table: "adventures",
                column: "TerminationScriptId");

            migrationBuilder.AddForeignKey(
                name: "FK_adventures_scripts_TerminationScriptId",
                table: "adventures",
                column: "TerminationScriptId",
                principalTable: "scripts",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_adventures_scripts_TerminationScriptId",
                table: "adventures");

            migrationBuilder.DropIndex(
                name: "IX_adventures_TerminationScriptId",
                table: "adventures");

            migrationBuilder.DropColumn(
                name: "TerminationScriptId",
                table: "adventures");
        }
    }
}
