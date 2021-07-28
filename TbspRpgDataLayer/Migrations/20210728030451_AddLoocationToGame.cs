using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TbspRpgDataLayer.Migrations
{
    public partial class AddLoocationToGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LocationId",
                table: "games",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_games_LocationId",
                table: "games",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_games_locations_LocationId",
                table: "games",
                column: "LocationId",
                principalTable: "locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_games_locations_LocationId",
                table: "games");

            migrationBuilder.DropIndex(
                name: "IX_games_LocationId",
                table: "games");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "games");
        }
    }
}
