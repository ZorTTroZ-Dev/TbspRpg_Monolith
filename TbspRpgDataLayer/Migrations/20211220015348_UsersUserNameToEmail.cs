using Microsoft.EntityFrameworkCore.Migrations;

namespace TbspRpgDataLayer.Migrations
{
    public partial class UsersUserNameToEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "user",
                newName: "Email");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Email",
                table: "user",
                newName: "UserName");
        }
    }
}
