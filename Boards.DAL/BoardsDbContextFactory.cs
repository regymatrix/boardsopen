using Boards.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace Boards.DAL
{


    public class BloggingContextFactory : IDesignTimeDbContextFactory<BoardsDbContext>
    {
        public BoardsDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BoardsDbContext>();
            optionsBuilder.UseMySql(Environment.GetEnvironmentVariable(Constants.DEFAULTCONENCTION_ENVIRONMENTVARIABLE));

            return new BoardsDbContext(optionsBuilder.Options);
        }


    }

}
