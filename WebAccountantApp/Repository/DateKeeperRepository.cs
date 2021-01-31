using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAccountantApp.Contracts;
using WebAccountantApp.Data;

namespace WebAccountantApp.Repository
{
    public class DateKeeperRepository : IDateKeeper
    {
        private readonly MyContext _db;
        public DateKeeperRepository(MyContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(DateKeeper entity)
        {
            await _db.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(DateKeeper entity)
        {
            _db.DateKeeper.Remove(entity);
            return await Save();
        }

        public async Task<bool> Exists(int id)
        {
            var exist = await _db.DateKeeper.AnyAsync(account => account.Id == id);
            return exist;
        }

        public async Task<IList<DateKeeper>> FindAll()
        {
            return await _db.DateKeeper.ToListAsync();
        }

        public async Task<DateKeeper> FindById(int? id)
        {
            return await _db.DateKeeper.SingleOrDefaultAsync(acc => acc.Id == id);
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(DateKeeper entity)
        {
            _db.DateKeeper.Update(entity);
            return await Save();
        }
    }
}
