using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TbspRpgDataLayer.Migrations
{
    public partial class UpdateUserForRegistration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "user",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLogin",
                table: "user",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RegistrationComplete",
                table: "user",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RegistrationKey",
                table: "user",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "user");

            migrationBuilder.DropColumn(
                name: "LastLogin",
                table: "user");

            migrationBuilder.DropColumn(
                name: "RegistrationComplete",
                table: "user");

            migrationBuilder.DropColumn(
                name: "RegistrationKey",
                table: "user");
        }
    }
}
