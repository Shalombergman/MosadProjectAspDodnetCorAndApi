using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MosadApiServer.Migrations
{
    /// <inheritdoc />
    public partial class RecreateLocationsTablesd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coordinates",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    x = table.Column<int>(type: "integer", nullable: false),
                    y = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coordinates", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Agents",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nickname = table.Column<string>(type: "text", nullable: false),
                    photo_url = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: true),
                    Coordinateid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agents", x => x.id);
                    table.ForeignKey(
                        name: "FK_Agents_Coordinates_Coordinateid",
                        column: x => x.Coordinateid,
                        principalTable: "Coordinates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Targets",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    position = table.Column<string>(type: "text", nullable: false),
                    photo_url = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: true),
                    coordinateid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Targets", x => x.id);
                    table.ForeignKey(
                        name: "FK_Targets_Coordinates_coordinateid",
                        column: x => x.coordinateid,
                        principalTable: "Coordinates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Missions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    timeLeft = table.Column<double>(type: "double precision", nullable: true),
                    ActualExecutionTime = table.Column<double>(type: "double precision", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: true),
                    agentId = table.Column<int>(type: "integer", nullable: true),
                    targetId = table.Column<int>(type: "integer", nullable: true)
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
                name: "IX_Agents_Coordinateid",
                table: "Agents",
                column: "Coordinateid");

            migrationBuilder.CreateIndex(
                name: "IX_Missions_agentId",
                table: "Missions",
                column: "agentId");

            migrationBuilder.CreateIndex(
                name: "IX_Missions_targetId",
                table: "Missions",
                column: "targetId");

            migrationBuilder.CreateIndex(
                name: "IX_Targets_coordinateid",
                table: "Targets",
                column: "coordinateid");
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
                name: "Coordinates");
        }
    }
}
