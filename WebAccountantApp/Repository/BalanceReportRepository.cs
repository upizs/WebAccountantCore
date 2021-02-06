using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAccountantApp.Contracts;
using WebAccountantApp.Data;

namespace WebAccountantApp.Repository
{
    public class BalanceReportRepository : IBalanceReport
    {
        private readonly MyContext _db;
        public BalanceReportRepository(MyContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(BalanceReport entity)
        {
            await _db.BalanceReports.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(BalanceReport entity)
        {
            _db.BalanceReports.Remove(entity);
            return await Save();
        }

        public async Task<bool> Exists(int id)
        {
            var exist = await _db.BalanceReports.AnyAsync(account => account.Id == id);
            return exist;
        }

        public async Task<IList<BalanceReport>> FindAll()
        {
            return await _db.BalanceReports.Include(report => report.Account).ToListAsync();
        }

        public async Task<BalanceReport> FindById(int? id)
        {
            return await _db.BalanceReports.SingleOrDefaultAsync(acc => acc.Id == id);
        }

        public async Task<IList<BalanceReport>> GetBalanceReportByMonth(int month, int year)
        {
            var reports = await _db.BalanceReports.Include(report => report.Account).Where(report => report.Date.Month == month && report.Date.Year == year).ToListAsync();
            return reports;
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(BalanceReport entity)
        {
            _db.BalanceReports.Update(entity);
            return await Save();
        }
    }
}
