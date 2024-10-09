using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Spotify_Autorization_Code_Flow.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class AddMultipleProvider : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bars",
                columns: table => new
                {
                    BarId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bars", x => x.BarId);
                });

            migrationBuilder.CreateTable(
                name: "BarProviders",
                columns: table => new
                {
                    BarProviderId = table.Column<Guid>(type: "uuid", nullable: false),
                    BarId = table.Column<int>(type: "integer", nullable: false),
                    ProviderId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccessToken = table.Column<string>(type: "text", nullable: false),
                    RefreshToken = table.Column<string>(type: "text", nullable: false),
                    TokenType = table.Column<string>(type: "text", nullable: false),
                    ExpiresIn = table.Column<int>(type: "integer", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BarProviders", x => x.BarProviderId);
                    table.ForeignKey(
                        name: "FK_BarProviders_Bars_BarId",
                        column: x => x.BarId,
                        principalTable: "Bars",
                        principalColumn: "BarId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Providers",
                columns: table => new
                {
                    ProviderId = table.Column<Guid>(type: "uuid", nullable: false),
                    BarProviderId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Providers", x => x.ProviderId);
                    table.ForeignKey(
                        name: "FK_Providers_BarProviders_BarProviderId",
                        column: x => x.BarProviderId,
                        principalTable: "BarProviders",
                        principalColumn: "BarProviderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BarProviders_BarId",
                table: "BarProviders",
                column: "BarId");

            migrationBuilder.CreateIndex(
                name: "IX_Providers_BarProviderId",
                table: "Providers",
                column: "BarProviderId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Providers");

            migrationBuilder.DropTable(
                name: "BarProviders");

            migrationBuilder.DropTable(
                name: "Bars");
        }
    }
}
