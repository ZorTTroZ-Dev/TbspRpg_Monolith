using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TbspRpgDataLayer.Migrations
{
    public partial class AlterRouteFieldNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FailureSourceKey",
                table: "routes");

            migrationBuilder.RenameColumn(
                name: "SuccessSourceKey",
                table: "routes",
                newName: "RouteTakenSourceKey");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RouteTakenSourceKey",
                table: "routes",
                newName: "SuccessSourceKey");

            migrationBuilder.AddColumn<Guid>(
                name: "FailureSourceKey",
                table: "routes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
