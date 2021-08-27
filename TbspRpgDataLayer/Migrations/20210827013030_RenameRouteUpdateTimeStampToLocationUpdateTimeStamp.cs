using Microsoft.EntityFrameworkCore.Migrations;

namespace TbspRpgDataLayer.Migrations
{
    public partial class RenameRouteUpdateTimeStampToLocationUpdateTimeStamp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RouteUpdateTimeStamp",
                table: "games",
                newName: "LocationUpdateTimeStamp");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LocationUpdateTimeStamp",
                table: "games",
                newName: "RouteUpdateTimeStamp");
        }
    }
}
