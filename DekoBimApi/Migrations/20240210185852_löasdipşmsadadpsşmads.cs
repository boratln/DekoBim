using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DekoBimApi.Migrations
{
    /// <inheritdoc />
    public partial class löasdipşmsadadpsşmads : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyUrl",
                table: "Companys",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FacebookUrl",
                table: "Companys",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstragramUrl",
                table: "Companys",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkedinUrl",
                table: "Companys",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyUrl",
                table: "Companys");

            migrationBuilder.DropColumn(
                name: "FacebookUrl",
                table: "Companys");

            migrationBuilder.DropColumn(
                name: "InstragramUrl",
                table: "Companys");

            migrationBuilder.DropColumn(
                name: "LinkedinUrl",
                table: "Companys");
        }
    }
}
