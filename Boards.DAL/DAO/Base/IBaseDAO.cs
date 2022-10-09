using System;
using System.Collections.Generic;
using System.Text;

namespace Boards.DAL.DAO.Base
{
    public interface IBaseDAO<T> where T : class
    {
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
        IEnumerable<T> Get();
        T Get(int id);
    }
}
