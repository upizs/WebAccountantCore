using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
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

            var expenseAccounts = await _accountRepo.GetAccountByType(AccountType.Expense);
            var incomeAccounts = await _accountRepo.GetAccountByType(AccountType.Income);
            

            //Create this weeks reports 
            var thisWeeksExpenseReports = CreateReports(AccountType.Expense, thisWeeksTransactions, expenseAccounts);
            var thisWeeksIncomeReports = CreateReports(AccountType.Income, thisWeeksTransactions, incomeAccounts);


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

            var expenseAccounts = await _accountRepo.GetAccountByType(AccountType.Expense);
            var incomeAccounts = await _accountRepo.GetAccountByType(AccountType.Income);

            var monthlyExpenseReports = CreateReports(AccountType.Expense, monthlyTransactions, expenseAccounts);
            var montlyIncomeReports = CreateReports(AccountType.Income, monthlyTransactions, incomeAccounts);

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

        //TODO Charts for Transactions (expense and income)
        //TODO Charts for Expense - Income difference by months.
        public async Task<ActionResult> Charts()
        {
            Random r = new Random();

            var transactions = await _transactionRepo.FindAll();
            var expenseAccounts = await _accountRepo.GetAccountByType(AccountType.Expense);
            var groupedReports = transactions.GroupBy(x => new
            {
                Month = x.Date.Month,
                Year = x.Date.Month
            }).Select(group => CreateReports(AccountType.Expense, group.ToList(), expenseAccounts));


            var datasets = new List<ChartObject>();

            foreach (var group in groupedReports)
            {
                var chartObject = new ChartObject
                {
                    Label = GetMonthName(group.FirstOrDefault().Month) + " " + group.FirstOrDefault().Year,
                    Data = group.Select(report => report.Value).ToArray(),
                    BackgroundColor = "rgba" + (r.Next(0, 256), r.Next(0, 256), r.Next(0, 256), 1).ToString()
                };
                datasets.Add(chartObject);
            }

            var chartViewModel = new ChartVM
            {
                Labels = groupedReports.FirstOrDefault().Select(x => x.Account.Name).ToArray(),
                Datasets = datasets.ToArray()
            };

            return View(chartViewModel);
        }

        //Made this faster by providing already sorted accounts like (expenseAccounts && incomeAccounts)
        private List<ReportVM> CreateReports(AccountType accountType, IList<Transaction> transactions, IList<Account> accounts)
        {
            var reports = new List<ReportVM>();
            foreach (var acc in accounts)
            {
               
                var thisAccountTransactions = new List<Transaction>();

                //Find all transactions for this account in given list
                //Looking for transactions where Debit id is Expense acc id, becuase Expense is being debited
                //and Income is being Credited in normal transaction.
                if (accountType == AccountType.Expense)
                    thisAccountTransactions = transactions.Where(x => x.DebitId == acc.Id).ToList();
                else if (accountType == AccountType.Income)
                    thisAccountTransactions = transactions.Where(x => x.CreditId == acc.Id).ToList();

                //if is rights account type and has transactions in given period, create report model for it
                if ( thisAccountTransactions.Any())
                {
                    var report = new ReportVM();
                    report.Account = acc;
                    //sum the value of transactions for this account
                    report.Value += thisAccountTransactions.Select(x => x.Value).Sum();
                    report.Month = thisAccountTransactions.FirstOrDefault().Date.Month;
                    report.Year = thisAccountTransactions.FirstOrDefault().Date.Year;
                    reports.Add(report);
                }
                else
                //Create a report object with zero value so I can still show it in charts. Otherwise the report for some months would exist
                //and that would ruin data sequence for charts.
                //TODO however this is useless and takes power for Report index and montly report (Need condition that only does this for charts)
                {
                    var report = new ReportVM
                    {
                        Account = acc,
                        Month = transactions.FirstOrDefault().Date.Month,
                        Year = transactions.FirstOrDefault().Date.Year,
                        Value = 0
                    };
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

        public string GetMonthName(int month)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
        }

    }
}
