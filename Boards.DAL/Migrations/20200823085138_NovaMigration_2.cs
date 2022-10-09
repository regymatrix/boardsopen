using Microsoft.EntityFrameworkCore.Migrations;

namespace Boards.DAL.Migrations
{
    public partial class NovaMigration_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVIP",
                table: "Usuarios",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVIP",
                table: "Usuarios");
        }
    }
}
