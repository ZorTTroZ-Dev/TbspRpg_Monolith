using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TbspRpgDataLayer.Migrations
{
    public partial class AddGameObjectsToEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "adventure_objects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    Type = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    NameSourceKey = table.Column<Guid>(type: "uuid", nullable: false),
                    DescriptionSourceKey = table.Column<Guid>(type: "uuid", nullable: false),
                    AdventureId = table.Column<Guid>(type: "uuid", nullable: false),
                    InitializationScriptId = table.Column<Guid>(type: "uuid", nullable: true),
                    ActionScriptId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_adventure_objects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_adventure_objects_adventures_AdventureId",
                        column: x => x.AdventureId,
                        principalTable: "adventures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_adventure_objects_scripts_ActionScriptId",
                        column: x => x.ActionScriptId,
                        principalTable: "scripts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_adventure_objects_scripts_InitializationScriptId",
                        column: x => x.InitializationScriptId,
                        principalTable: "scripts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "adventure_object_game_states",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false),
                    AdventureObjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    AdventureObjectState = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_adventure_object_game_states", x => x.Id);
                    table.ForeignKey(
                        name: "FK_adventure_object_game_states_adventure_objects_AdventureObj~",
                        column: x => x.AdventureObjectId,
                        principalTable: "adventure_objects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_adventure_object_game_states_games_GameId",
                        column: x => x.GameId,
                        principalTable: "games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdventureObjectLocation",
                columns: table => new
                {
                    AdventureObjectsId = table.Column<Guid>(type: "uuid", nullable: false),
                    LocationsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdventureObjectLocation", x => new { x.AdventureObjectsId, x.LocationsId });
                    table.ForeignKey(
                        name: "FK_AdventureObjectLocation_adventure_objects_AdventureObjectsId",
                        column: x => x.AdventureObjectsId,
                        principalTable: "adventure_objects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdventureObjectLocation_locations_LocationsId",
                        column: x => x.LocationsId,
                        principalTable: "locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_adventure_object_game_states_AdventureObjectId",
                table: "adventure_object_game_states",
                column: "AdventureObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_adventure_object_game_states_GameId",
                table: "adventure_object_game_states",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_adventure_objects_ActionScriptId",
                table: "adventure_objects",
                column: "ActionScriptId");

            migrationBuilder.CreateIndex(
                name: "IX_adventure_objects_AdventureId",
                table: "adventure_objects",
                column: "AdventureId");

            migrationBuilder.CreateIndex(
                name: "IX_adventure_objects_InitializationScriptId",
                table: "adventure_objects",
                column: "InitializationScriptId");

            migrationBuilder.CreateIndex(
                name: "IX_AdventureObjectLocation_LocationsId",
                table: "AdventureObjectLocation",
                column: "LocationsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "adventure_object_game_states");

            migrationBuilder.DropTable(
                name: "AdventureObjectLocation");

            migrationBuilder.DropTable(
                name: "adventure_objects");
        }
    }
}
