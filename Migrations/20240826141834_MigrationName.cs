using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MosadApiServer.Migrations
{
    /// <inheritdoc />
    public partial class MigrationName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    x = table.Column<int>(type: "int", nullable: false),
                    y = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Agents",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nickname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    photo_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<int>(type: "int", nullable: true),
                    locationid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agents", x => x.id);
                    table.ForeignKey(
                        name: "FK_Agents_Locations_locationid",
                        column: x => x.locationid,
                        principalTable: "Locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Targets",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    photo_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<int>(type: "int", nullable: true),
                    locationid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Targets", x => x.id);
                    table.ForeignKey(
                        name: "FK_Targets_Locations_locationid",
                        column: x => x.locationid,
                        principalTable: "Locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Missions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    timeLeft = table.Column<double>(type: "float", nullable: true),
                    ActualExecutionTime = table.Column<double>(type: "float", nullable: true),
                    status = table.Column<int>(type: "int", nullable: true),
                    agentId = table.Column<int>(type: "int", nullable: true),
                    targetId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Missions", x => x.id);
                    table.ForeignKey(
                        name: "FK_Missions_Agents_agentId",
                        column: x => x.agentId,
                        principalTable: "Agents",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Missions_Targets_targetId",
                        column: x => x.targetId,
                        principalTable: "Targets",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Agents_locationid",
                table: "Agents",
                column: "locationid");

            migrationBuilder.CreateIndex(
                name: "IX_Missions_agentId",
                table: "Missions",
                column: "agentId");

            migrationBuilder.CreateIndex(
                name: "IX_Missions_targetId",
                table: "Missions",
                column: "targetId");

            migrationBuilder.CreateIndex(
                name: "IX_Targets_locationid",
                table: "Targets",
                column: "locationid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Missions");

            migrationBuilder.DropTable(
                name: "Agents");

            migrationBuilder.DropTable(
                name: "Targets");

            migrationBuilder.DropTable(
                name: "Locations");
        }
    }
}
