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
        //changed return type to bool so that I can return it in other methods.
        //async void shouldnt be used because cant wait for complition and cant handle exceptions.
        Task<bool> Exists(int id);
        Task<bool> Create(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(T entity);
        Task<bool> Save();
    }
}
