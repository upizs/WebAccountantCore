using AutoMapper;
using Microsoft.AspNetCore.Http;
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
    public class ReportController : Controller
    {

        #region Dependency Injuction

        private readonly ITransactionRepository _transactionRepo;
        private readonly IAccountRepository _accountRepo;
        private readonly IMapper _mapper;

        public ReportController(ITransactionRepository transactionRepo, IAccountRepository accountRepo, IMapper mapper)
        {
            _transactionRepo = transactionRepo;
            _accountRepo = accountRepo;
            _mapper = mapper;
        }
        #endregion

        // GET: ReportController
        public async Task<ActionResult> Index()
        {
            //Archive all transactions in months and years to create a report navigation bar
            //The user then can view transactions by months and years
            var archivedTransactions = await ArchiveTransactions();

            //Get the list of all accounts for the view
            var accounts = await _accountRepo.FindAll();

            //Get the date of the last Friday, because I am getting paid on fridays
            //and I would like my short term Reports to be from Friday to Thursday
            DateTime lastFridayDate = GetLastFriday();

            
            //Get all the transactions since last friday
            //This is for the weekly report, only available for this week for now
            var thisWeeksTransactions = await _transactionRepo.FilterTransacations(lastFridayDate);

            

            //Create this weeks reports 
            var thisWeeksExpenseReports = CreateReports(AccountType.Expense, thisWeeksTransactions, accounts);
            var thisWeeksIncomeReports = CreateReports(AccountType.Income, thisWeeksTransactions, accounts);


            //create a report viewModel
            var reportViewModel = new ListReportVM()
            {
                Accounts = accounts,
                IncomeReports = thisWeeksIncomeReports,
                ExpenseReports = thisWeeksExpenseReports,
                Archives = archivedTransactions
            };


            return View(reportViewModel);
        }

        public async Task<ActionResult> MontlyReport(int year, int month)
        {
            var accounts = await _accountRepo.FindAll();
            var archivedTransactions = await ArchiveTransactions();

            var monthlyTransactions = await _transactionRepo.GetMontlyTransactions(month, year);

            var monthlyExpenseReports = CreateReports(AccountType.Expense, monthlyTransactions, accounts);
            var montlyIncomeReports = CreateReports(AccountType.Income, monthlyTransactions, accounts);

            var reportViewModel = new ListReportVM()
            {
                Accounts = accounts,
                IncomeReports = montlyIncomeReports,
                ExpenseReports = monthlyExpenseReports,
                Archives = archivedTransactions

            };

            //use the same view as no need for different
            return View(nameof(Index), reportViewModel);
        }

        //Maybe could make this faster by providing already sorted accounts like (expenseAccount && incomeAccounts)
        //Instead of giving accountType I could just give sorted accounts check the type in method and act accordingly
        private List<ReportVM> CreateReports(AccountType accountType, IList<Transaction> transactions, IList<Account> accounts)
        {
            var reports = new List<ReportVM>();
            foreach (var acc in accounts)
            {
                var isRightType = acc.AccountType == accountType;
                var thisAccountTransactions = new List<Transaction>();

                //Find all transactions for this account in given list
                //If Account Type is Expense then we are looking for accounts debited, because Expense can be Expense only when debited, if credited means it would be a refund
                //If Account Type is Income then we are looking for account credited, becayse Income can only be Credited. 
                if (accountType == AccountType.Expense)
                    thisAccountTransactions = transactions.Where(x => x.DebitId == acc.Id).ToList();
                else if (accountType == AccountType.Income)
                    thisAccountTransactions = transactions.Where(x => x.CreditId == acc.Id).ToList();

                //if is rights account type and has transactions in given period, create report model for it
                if (isRightType && thisAccountTransactions.Any())
                {
                    var report = new ReportVM();
                    report.Account = acc;
                    //sum the value of transactions for this account
                    report.Value += thisAccountTransactions.Select(x => x.Value).Sum();
                    reports.Add(report);
                }
            }
            return reports;
        }

        

        private async Task<List<ArchiveEntry>> ArchiveTransactions()
        {
            var transactions = await _transactionRepo.FindAll();
                
            var archived = transactions.GroupBy(x => new
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

        private DateTime GetLastFriday()
        {
            var date = DateTime.Now.Date;
            while (date.DayOfWeek != DayOfWeek.Friday)
            {
                date = date.AddDays(-1);
            }

            return date;
        }

    }
}
