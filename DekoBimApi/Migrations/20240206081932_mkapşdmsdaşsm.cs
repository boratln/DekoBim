using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DekoBimApi.Migrations
{
    /// <inheritdoc />
    public partial class mkapşdmsdaşsm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "teknik_ozellik",
                table: "Products",
                newName: "teknik_ozellikdata");

            migrationBuilder.AlterColumn<string>(
                name: "Name_",
                table: "FileTypes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "teknikozellik",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "teknikozellik",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "teknik_ozellikdata",
                table: "Products",
                newName: "teknik_ozellik");

            migrationBuilder.AlterColumn<string>(
                name: "Name_",
                table: "FileTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
