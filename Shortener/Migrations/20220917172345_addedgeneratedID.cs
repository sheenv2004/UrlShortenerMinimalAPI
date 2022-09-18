using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shortener.Migrations
{
    public partial class addedgeneratedID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "generatedID",
                table: "Urls",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "generatedID",
                table: "Urls");
        }
    }
}
