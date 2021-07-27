using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TbspRpgDataLayer.Migrations
{
    public partial class AddUserAndAdventureEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "adventures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    Name = table.Column<string>(type: "text", nullable: true),
                    SourceKey = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_adventures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "sources_en",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    Key = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Text = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sources_en", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "sources_esp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    Key = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Text = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sources_esp", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    UserName = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "locations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Initial = table.Column<bool>(type: "boolean", nullable: false),
                    SourceKey = table.Column<Guid>(type: "uuid", nullable: false),
                    AdventureId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_locations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_locations_adventures_AdventureId",
                        column: x => x.AdventureId,
                        principalTable: "adventures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "routes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    Name = table.Column<string>(type: "text", nullable: true),
                    SuccessSourceKey = table.Column<Guid>(type: "uuid", nullable: false),
                    FailureSourceKey = table.Column<Guid>(type: "uuid", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    DestinationLocationId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_routes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_routes_locations_DestinationLocationId",
                        column: x => x.DestinationLocationId,
                        principalTable: "locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_routes_locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_locations_AdventureId",
                table: "locations",
                column: "AdventureId");

            migrationBuilder.CreateIndex(
                name: "IX_routes_DestinationLocationId",
                table: "routes",
                column: "DestinationLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_routes_LocationId",
                table: "routes",
                column: "LocationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "routes");

            migrationBuilder.DropTable(
                name: "sources_en");

            migrationBuilder.DropTable(
                name: "sources_esp");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "locations");

            migrationBuilder.DropTable(
                name: "adventures");
        }
    }
}
