using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DekoBimApi.Migrations
{
    /// <inheritdoc />
    public partial class öaldpşsmsadslp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
       name: "IsExistRevit",
       table: "Products",
       type: "bit",
       nullable: true,
       defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Isexistifcformat",
                table: "Products",
                type: "bit",
                nullable: true,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isExistAutocad",
                table: "Products",
                type: "bit",
                nullable: true,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isExistSolidworks",
                table: "Products",
                type: "bit",
                nullable: true,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsExistRevit",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Isexistifcformat",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "isExistAutocad",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "isExistSolidworks",
                table: "Products");
        }
    }
}
