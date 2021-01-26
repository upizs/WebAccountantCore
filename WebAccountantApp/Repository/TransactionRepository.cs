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
        public async Task<bool> Create(Transaction entity)
        {
            await _db.Transactions.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Transaction entity)
        {
            _db.Transactions.Remove(entity);
            return await Save();
        }

        public async Task<bool> Exists(int id)
        {
            var exist = await _db.Transactions.AnyAsync(transaction => transaction.Id == id);
            return exist;
        }

        public async Task<IList<Transaction>> FindAll()
        {
            return await _db.Transactions.Include(tran => tran.Debit).Include(tran => tran.Credit).ToListAsync();
        }

        public async Task<Transaction> FindById(int? id)
        {
            return await _db.Transactions.SingleOrDefaultAsync(tr => tr.Id == id);
        }


        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(Transaction entity)
        {
            _db.Transactions.Update(entity);
            return await Save();
        }
    }
}
