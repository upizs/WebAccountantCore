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

            var lastMonthsBalanceReport = await _balanceRepo.GetBalanceReportByMonth(todaysDate.Month - 1, todaysDate.Year);

            var mappedReports = _mapper.Map<List<BalanceReportVM>>(lastMonthsBalanceReport);

            var model = new ListBalanceReportVM
            {
                BalanceReports = mappedReports,
                Archives = archivedBalanceReports
            };

            return View(model);
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
