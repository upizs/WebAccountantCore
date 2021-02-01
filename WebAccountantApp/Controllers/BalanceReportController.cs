using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAccountantApp.Contracts;

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
            var balanceReports = await _balanceRepo.FindAll();
            //will use the same archive method like in Reports
            //Index will show the last month. Can choose to see other months
            //Also need a view with chart that is comparing the months. 
            return View();
        }
    }
}
