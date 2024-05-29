using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DekoBimApi.Migrations
{
    /// <inheritdoc />
    public partial class makpşdskmdaspşadsm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {



            migrationBuilder.AddColumn<string>(
          name: "tanitimvideopath",
          table: "Products",
          type: "varchar(max)",
          nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "montajvideopath",
                table: "Products",
                type: "varchar(max)",
                nullable: true);




        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
          
        }
    }
}
