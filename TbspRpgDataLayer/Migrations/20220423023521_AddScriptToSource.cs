using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TbspRpgDataLayer.Migrations
{
    public partial class AddScriptToSource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ScriptId",
                table: "sources_esp",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ScriptId",
                table: "sources_en",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_sources_esp_ScriptId",
                table: "sources_esp",
                column: "ScriptId");

            migrationBuilder.CreateIndex(
                name: "IX_sources_en_ScriptId",
                table: "sources_en",
                column: "ScriptId");

            migrationBuilder.AddForeignKey(
                name: "FK_sources_en_scripts_ScriptId",
                table: "sources_en",
                column: "ScriptId",
                principalTable: "scripts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_sources_esp_scripts_ScriptId",
                table: "sources_esp",
                column: "ScriptId",
                principalTable: "scripts",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sources_en_scripts_ScriptId",
                table: "sources_en");

            migrationBuilder.DropForeignKey(
                name: "FK_sources_esp_scripts_ScriptId",
                table: "sources_esp");

            migrationBuilder.DropIndex(
                name: "IX_sources_esp_ScriptId",
                table: "sources_esp");

            migrationBuilder.DropIndex(
                name: "IX_sources_en_ScriptId",
                table: "sources_en");

            migrationBuilder.DropColumn(
                name: "ScriptId",
                table: "sources_esp");

            migrationBuilder.DropColumn(
                name: "ScriptId",
                table: "sources_en");
        }
    }
}
