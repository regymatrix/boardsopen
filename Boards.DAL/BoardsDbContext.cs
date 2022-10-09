using Boards.DTO;
using Microsoft.EntityFrameworkCore;

namespace Boards.DAL
{
    public class BoardsDbContext : DbContext
    {

        public BoardsDbContext(DbContextOptions<BoardsDbContext> options) : base(options)
        {

        }
      

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Quadro> Quadros { get; set; }
        public DbSet<Cartao> Cartoes { get; set; }
        public DbSet<Lead> Leads { get; set; }
        public DbSet<Administrador> Administradores { get; set; }
        public DbSet<Configuracao> Configuracaos { get; set; }


    }
}
