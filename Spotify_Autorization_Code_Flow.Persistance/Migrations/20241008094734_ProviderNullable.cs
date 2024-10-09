using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spotify_Autorization_Code_Flow.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class ProviderNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Providers_BarProviders_BarProviderId",
                table: "Providers");

            migrationBuilder.AlterColumn<Guid>(
                name: "BarProviderId",
                table: "Providers",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Providers_BarProviders_BarProviderId",
                table: "Providers",
                column: "BarProviderId",
                principalTable: "BarProviders",
                principalColumn: "BarProviderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Providers_BarProviders_BarProviderId",
                table: "Providers");

            migrationBuilder.AlterColumn<Guid>(
                name: "BarProviderId",
                table: "Providers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Providers_BarProviders_BarProviderId",
                table: "Providers",
                column: "BarProviderId",
                principalTable: "BarProviders",
                principalColumn: "BarProviderId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
