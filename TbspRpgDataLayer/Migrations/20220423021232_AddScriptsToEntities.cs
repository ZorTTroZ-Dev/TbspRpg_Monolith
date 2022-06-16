using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TbspRpgDataLayer.Migrations
{
    public partial class AddScriptsToEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RouteTakenScriptId",
                table: "routes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EnterScriptId",
                table: "locations",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ExitScriptId",
                table: "locations",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "InitializationScriptId",
                table: "adventures",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_routes_RouteTakenScriptId",
                table: "routes",
                column: "RouteTakenScriptId");

            migrationBuilder.CreateIndex(
                name: "IX_locations_EnterScriptId",
                table: "locations",
                column: "EnterScriptId");

            migrationBuilder.CreateIndex(
                name: "IX_locations_ExitScriptId",
                table: "locations",
                column: "ExitScriptId");

            migrationBuilder.CreateIndex(
                name: "IX_adventures_InitializationScriptId",
                table: "adventures",
                column: "InitializationScriptId");

            migrationBuilder.AddForeignKey(
                name: "FK_adventures_scripts_InitializationScriptId",
                table: "adventures",
                column: "InitializationScriptId",
                principalTable: "scripts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_locations_scripts_EnterScriptId",
                table: "locations",
                column: "EnterScriptId",
                principalTable: "scripts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_locations_scripts_ExitScriptId",
                table: "locations",
                column: "ExitScriptId",
                principalTable: "scripts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_routes_scripts_RouteTakenScriptId",
                table: "routes",
                column: "RouteTakenScriptId",
                principalTable: "scripts",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_adventures_scripts_InitializationScriptId",
                table: "adventures");

            migrationBuilder.DropForeignKey(
                name: "FK_locations_scripts_EnterScriptId",
                table: "locations");

            migrationBuilder.DropForeignKey(
                name: "FK_locations_scripts_ExitScriptId",
                table: "locations");

            migrationBuilder.DropForeignKey(
                name: "FK_routes_scripts_RouteTakenScriptId",
                table: "routes");

            migrationBuilder.DropIndex(
                name: "IX_routes_RouteTakenScriptId",
                table: "routes");

            migrationBuilder.DropIndex(
                name: "IX_locations_EnterScriptId",
                table: "locations");

            migrationBuilder.DropIndex(
                name: "IX_locations_ExitScriptId",
                table: "locations");

            migrationBuilder.DropIndex(
                name: "IX_adventures_InitializationScriptId",
                table: "adventures");

            migrationBuilder.DropColumn(
                name: "RouteTakenScriptId",
                table: "routes");

            migrationBuilder.DropColumn(
                name: "EnterScriptId",
                table: "locations");

            migrationBuilder.DropColumn(
                name: "ExitScriptId",
                table: "locations");

            migrationBuilder.DropColumn(
                name: "InitializationScriptId",
                table: "adventures");
        }
    }
}
