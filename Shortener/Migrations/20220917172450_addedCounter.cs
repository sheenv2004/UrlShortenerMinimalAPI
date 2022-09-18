using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shortener.Migrations
{
    public partial class addedCounter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "generatedID",
                table: "Urls",
                newName: "counter");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "counter",
                table: "Urls",
                newName: "generatedID");
        }
    }
}
