using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAccountantApp.Contracts;
using WebAccountantApp.Data;

namespace WebAccountantApp.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly MyContext _db;
        public TransactionRepository(MyContext db)
        {
            _db = db;
        }
        public async Task Create(Transaction entity)
        {
            await _db.Transactions.AddAsync(entity);
            await Save();
        }

        public async void Delete(Transaction entity)
        {
            _db.Transactions.Remove(entity);
            await Save();
        }

        public async Task<bool> Exists(int id)
        {
            var exist = await _db.Transactions.AnyAsync(transaction => transaction.Id == id);
            return exist;
        }

        public async Task<IList<Transaction>> FindAll()
        {
            return await _db.Transactions.ToListAsync();
        }

        public async Task<Transaction> FindById(int id)
        {
            return await _db.Transactions.FindAsync(id);
        }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }

        public async void Update(Transaction entity)
        {
            _db.Transactions.Update(entity);
            await Save();
        }
    }
}
