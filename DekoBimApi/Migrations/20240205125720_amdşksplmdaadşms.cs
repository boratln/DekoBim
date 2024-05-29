using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DekoBimApi.Migrations
{
    /// <inheritdoc />
    public partial class amdşksplmdaadşms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FileTypeId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_FileTypeId",
                table: "Products",
                column: "FileTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_FileTypes_FileTypeId",
                table: "Products",
                column: "FileTypeId",
                principalTable: "FileTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_FileTypes_FileTypeId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_FileTypeId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "FileTypeId",
                table: "Products");
        }
    }
}
