using Microsoft.EntityFrameworkCore.Migrations;

namespace Boards.DAL.Migrations
{
    public partial class NovaMigration_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "Quadros",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BackgroundColor",
                table: "Cartoes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nome",
                table: "Quadros");

            migrationBuilder.DropColumn(
                name: "BackgroundColor",
                table: "Cartoes");
        }
    }
}
