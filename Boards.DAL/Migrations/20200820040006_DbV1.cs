using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Boards.DAL.Migrations
{
    public partial class DbV1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(nullable: true),
                    Nome = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Quadros",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Url = table.Column<string>(nullable: true),
                    Id_Usuario = table.Column<int>(nullable: false),
                    Data_Criacao = table.Column<DateTime>(nullable: false),
                    Is_Aberto = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quadros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quadros_Usuarios_Id_Usuario",
                        column: x => x.Id_Usuario,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cartoes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_Usuario = table.Column<int>(nullable: false),
                    Id_Quadro = table.Column<int>(nullable: false),
                    Conteudo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cartoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cartoes_Quadros_Id_Quadro",
                        column: x => x.Id_Quadro,
                        principalTable: "Quadros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cartoes_Usuarios_Id_Usuario",
                        column: x => x.Id_Usuario,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cartoes_Id_Quadro",
                table: "Cartoes",
                column: "Id_Quadro");

            migrationBuilder.CreateIndex(
                name: "IX_Cartoes_Id_Usuario",
                table: "Cartoes",
                column: "Id_Usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Quadros_Id_Usuario",
                table: "Quadros",
                column: "Id_Usuario");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cartoes");

            migrationBuilder.DropTable(
                name: "Quadros");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
