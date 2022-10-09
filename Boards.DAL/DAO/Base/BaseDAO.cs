using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boards.DAL.DAO.Base
{
    public class BaseDAO<T> : IBaseDAO<T> where T : class
    {
        public BoardsDbContext DbContext;
        private DbContextOptions<BoardsDbContext> DbContextOptions;
        public BaseDAO()
        {
            var optionsBuilder = new DbContextOptionsBuilder<BoardsDbContext>();
            this.DbContextOptions = optionsBuilder
                .UseMySql(Environment.GetEnvironmentVariable(Constants.DEFAULTCONENCTION_ENVIRONMENTVARIABLE)).Options;
            DbContext = new BoardsDbContext(this.DbContextOptions);
        }
        public void Add(T entity)
        {
            using (DbContext = new BoardsDbContext(DbContextOptions))
            {
                DbContext.Set<T>().Add(entity);
                DbContext.SaveChanges();
            }
        }

        public IEnumerable<T> GetWithIncludes(string[] includes)
        {
            using (DbContext = new BoardsDbContext(DbContextOptions))
            {
                var currentContext = DbContext.Set<T>().AsQueryable();
                foreach (var include in includes)
                    currentContext = currentContext.Include(include);

                return currentContext.ToList();
            }
        }



        public List<object> Include(string className)
        {
            using (DbContext = new BoardsDbContext(DbContextOptions))
            {
                return DbContext.Set<T>().Include(className).ToList() as List<object>;
            }
        }





        public IEnumerable<T> Get()
        {
            using (DbContext = new BoardsDbContext(DbContextOptions))
            {
                return DbContext.Set<T>().ToList();
            }
        }

        public T Get(int id)
        {
            using (DbContext = new BoardsDbContext(DbContextOptions))
            {
                return DbContext.Set<T>().Find(id);
            }
        }

        public T Get(string id)
        {
            using (DbContext = new BoardsDbContext(DbContextOptions))
            {
                return DbContext.Set<T>().Find(id);
            }
        }

        public void Delete(T entity)
        {
            using (DbContext = new BoardsDbContext(DbContextOptions))
            {
                DbContext.Set<T>().Remove(entity);
                DbContext.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (DbContext = new BoardsDbContext(DbContextOptions))
            {
                DbContext.Set<T>().Remove(DbContext.Set<T>().Find(id));
                DbContext.SaveChanges();
            }
        }
        public void Delete(string valor)
        {
            using (DbContext = new BoardsDbContext(DbContextOptions))
            {
                DbContext.Set<T>().Remove(DbContext.Set<T>().Find(valor));
                DbContext.SaveChanges();
            }
        }

        public void Update(T entity)
        {
            using (DbContext = new BoardsDbContext(DbContextOptions))
            {
                DbContext.Set<T>().Update(entity);
                DbContext.SaveChanges();
            }
        }
    }
}
