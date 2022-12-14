// <auto-generated />
using System;
using Boards.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Boards.DAL.Migrations
{
    [DbContext(typeof(BoardsDbContext))]
    [Migration("20200826052318_NovaMigration_3")]
    partial class NovaMigration_3
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Boards.DTO.Cartao", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("BackgroundColor")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Conteudo")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("Id_Quadro")
                        .HasColumnType("int");

                    b.Property<int>("Id_Usuario")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Id_Quadro");

                    b.HasIndex("Id_Usuario");

                    b.ToTable("Cartoes");
                });

            modelBuilder.Entity("Boards.DTO.Lead", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("VirouCliente")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.ToTable("Leads");
                });

            modelBuilder.Entity("Boards.DTO.Quadro", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Data_Criacao")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Id_Usuario")
                        .HasColumnType("int");

                    b.Property<bool>("Is_Aberto")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Nome")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Url")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("Id_Usuario");

                    b.ToTable("Quadros");
                });

            modelBuilder.Entity("Boards.DTO.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("IsVIP")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Nome")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("Boards.DTO.Cartao", b =>
                {
                    b.HasOne("Boards.DTO.Quadro", "Quadro")
                        .WithMany("Cartoes")
                        .HasForeignKey("Id_Quadro")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Boards.DTO.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("Id_Usuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Boards.DTO.Quadro", b =>
                {
                    b.HasOne("Boards.DTO.Usuario", "Usuario")
                        .WithMany("Quadros")
                        .HasForeignKey("Id_Usuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
