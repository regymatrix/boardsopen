using Microsoft.EntityFrameworkCore.Migrations;

namespace Boards.DAL.Migrations
{
    public partial class NovaMigration_6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsGod",
                table: "Usuarios",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsGod",
                table: "Usuarios");
        }
    }
}
