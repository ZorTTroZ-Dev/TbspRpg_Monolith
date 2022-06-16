using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TbspRpgDataLayer.Migrations
{
    public partial class AddReturnTypeToScript : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "ReturnType",
                table: "scripts",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "RouteTakenScriptId",
                table: "routes",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "ExitScriptId",
                table: "locations",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "EnterScriptId",
                table: "locations",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "InitializationScriptId",
                table: "adventures",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

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

            migrationBuilder.DropColumn(
                name: "ReturnType",
                table: "scripts");

            migrationBuilder.AlterColumn<Guid>(
                name: "RouteTakenScriptId",
                table: "routes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ExitScriptId",
                table: "locations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "EnterScriptId",
                table: "locations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "InitializationScriptId",
                table: "adventures",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_adventures_scripts_InitializationScriptId",
                table: "adventures",
                column: "InitializationScriptId",
                principalTable: "scripts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_locations_scripts_EnterScriptId",
                table: "locations",
                column: "EnterScriptId",
                principalTable: "scripts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_locations_scripts_ExitScriptId",
                table: "locations",
                column: "ExitScriptId",
                principalTable: "scripts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_routes_scripts_RouteTakenScriptId",
                table: "routes",
                column: "RouteTakenScriptId",
                principalTable: "scripts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
