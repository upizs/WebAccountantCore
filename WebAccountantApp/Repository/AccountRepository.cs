using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAccountantApp.Contracts;
using WebAccountantApp.Data;

namespace WebAccountantApp.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly MyContext _db;
        public AccountRepository(MyContext db)
        {
            _db = db;
        }
        public async Task<bool> Create(Account entity)
        {
            await _db.Accounts.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Account entity)
        {
            _db.Accounts.Remove(entity);
            return await Save();
        }

        public async Task<bool> Exists(int id)
        {
            var exist = await _db.Accounts.AnyAsync(account => account.Id == id);
            return exist;
        }

        public async Task<IList<Account>> FindAll()
        {
            return await _db.Accounts.ToListAsync();
        }

        public async Task<Account> FindById(int id)
        {
            return await _db.Accounts.FindAsync(id);
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(Account entity)
        {
            _db.Accounts.Update(entity);
            return await Save();
        }
    }
}
