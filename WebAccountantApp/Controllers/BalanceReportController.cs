using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAccountantApp.Contracts;
using WebAccountantApp.Data;
using WebAccountantApp.Models;

namespace WebAccountantApp.Controllers
{
    public class BalanceReportController : Controller
    {
        private readonly IBalanceReport _balanceRepo;
        private readonly IMapper _mapper;

        public BalanceReportController(IMapper mapper, IBalanceReport balanceReport)
        {
            _balanceRepo = balanceReport;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var archivedBalanceReports = await ArchiveReports();
            var todaysDate = DateTime.Now;


            var lastMonthsBalanceReports = await _balanceRepo.GetBalanceReportByMonth(todaysDate.Month - 1, todaysDate.Year);
            
            var mappedReports = _mapper.Map<List<BalanceReportVM>>(lastMonthsBalanceReports);
            //create separete lists for debit and credit, so I can have a better view of my accounts
            List<BalanceReportVM> debitReports = new List<BalanceReportVM>();
            List<BalanceReportVM> creditReports = new List<BalanceReportVM>();

            foreach (var report in mappedReports)
            {
                if (report.Account.AccountType == AccountType.Debit)
                    debitReports.Add(report);
                else
                    creditReports.Add(report);
            }

            var model = new ListBalanceReportVM
            {
                DebitBalanceReports = debitReports,
                CreditBalanceReports = creditReports,
                Archives = archivedBalanceReports
            };

            return View(model);
        }

        public async Task<IActionResult> ReportByMonth(int year, int month)
        {
            var archivedBalanceReports = await ArchiveReports();

            var lastMonthsBalanceReports = await _balanceRepo.GetBalanceReportByMonth(month, year);


            var mappedReports = _mapper.Map<List<BalanceReportVM>>(lastMonthsBalanceReports);

            //create separete lists for debit and credit, so I can have a better view of my accounts
            List<BalanceReportVM> debitReports = new List<BalanceReportVM>();
            List<BalanceReportVM> creditReports = new List<BalanceReportVM>();

            foreach (var report in mappedReports)
            {
                if (report.Account.AccountType == AccountType.Debit)
                    debitReports.Add(report);
                else
                    creditReports.Add(report);
            }


            var model = new ListBalanceReportVM
            {
                DebitBalanceReports = debitReports,
                CreditBalanceReports = creditReports,
                Archives = archivedBalanceReports
            };

            return View("Index",model);
        }

        private async Task<List<ArchiveEntry>> ArchiveReports()
        {
            var balanceReports = await _balanceRepo.FindAll();

            var archived = balanceReports.GroupBy(x => new
            {
                Month = x.Date.Month,
                Year = x.Date.Year
            })
                //create new archive entry for each group
                .Select(o => new ArchiveEntry
                {
                    Month = o.Key.Month,
                    Year = o.Key.Year
                })
                .OrderByDescending(a => a.Year)
                .ThenByDescending(a => a.Month)
                .ToList();

            return archived;
        }

    }
}
