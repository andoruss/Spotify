using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spotify_Autorization_Code_Flow.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class AddNameProvider : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Providers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Providers");
        }
    }
}
