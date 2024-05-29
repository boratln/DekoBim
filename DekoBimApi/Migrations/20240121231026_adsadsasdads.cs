using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DekoBimApi.Migrations
{
    /// <inheritdoc />
    public partial class adsadsasdads : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "katalogfiyatliste",
                table: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "katalogfiyatliste",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
