using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TbspRpgDataLayer.Migrations
{
    public partial class AddCreatedByToAdventure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByUserId",
                table: "adventures",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_adventures_CreatedByUserId",
                table: "adventures",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_adventures_user_CreatedByUserId",
                table: "adventures",
                column: "CreatedByUserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_adventures_user_CreatedByUserId",
                table: "adventures");

            migrationBuilder.DropIndex(
                name: "IX_adventures_CreatedByUserId",
                table: "adventures");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "adventures");
        }
    }
}
