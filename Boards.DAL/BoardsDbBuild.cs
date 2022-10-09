using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Boards.DAL
{
    public class BoardsDbBuild 
    {
        public static void Build(IApplicationBuilder app)
        {
            Migrate(app.ApplicationServices.GetRequiredService<BoardsDbContext>());
        }

        public static void Migrate(BoardsDbContext context)
        {
            context.Database.Migrate();
        }
    }
}
