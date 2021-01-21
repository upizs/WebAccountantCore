using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAccountantApp.Contracts
{
    public interface IBaseRepository<T> where T :class
    {
        Task<IList<T>> FindAll();
        Task<T> FindById(int id);

        Task<bool> Exists(int id);
        Task Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task Save();
    }
}
