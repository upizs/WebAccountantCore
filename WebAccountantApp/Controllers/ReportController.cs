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
            //random instance for creating color codes for charts
            Random r = new Random();

            //All transactions to split for charts
            var transactions = await _transactionRepo.FindAll();

            //get all the expense and income accounts separetly
            var expenseAccounts = await _accountRepo.GetAccountByType(AccountType.Expense);
            var incomeAccounts = await _accountRepo.GetAccountByType(AccountType.Income);

            //Create and group by months expense and income reports 
            var groupedExpenseReports = transactions.GroupBy(x => new
            {
                Month = x.Date.Month,
                Year = x.Date.Month
            }).Select(group => CreateReports(AccountType.Expense, group.ToList(), expenseAccounts));

            var groupedIncomeReports = transactions.GroupBy(x => new
            {
                Month = x.Date.Month,
                Year = x.Date.Month
            }).Select(group => CreateReports(AccountType.Income, group.ToList(), incomeAccounts));

            //Created grouped by months Totals of Expenses and Income 
            var groupedTotalExpense = groupedExpenseReports.Select(x => x.Select(b => b.Value).Sum()).ToArray();
            var groupedTotalIncome = groupedIncomeReports.Select(x => x.Select(b => b.Value).Sum()).ToArray();

            //TODO: create chartObjects with totals and difference
            var totalExpenseChartObject = new ChartObject 
            {
                BackgroundColor = ConsoleColor.Red.ToString(),
                Data = groupedTotalExpense,
                Label = "Total Expense"

            };
            var totalIncomeChartObject = new ChartObject
            {
                BackgroundColor = ConsoleColor.Green.ToString(),
                Data = groupedTotalIncome,
                Label = "Total Income"

            };

            var profitOrLossArray = new decimal[groupedTotalExpense.Count()];
            for (int i = 0; i< groupedTotalExpense.Count(); i++)
            {
                decimal profitOrLoss = groupedTotalIncome[i] - groupedTotalExpense[i];
                profitOrLossArray[i] = profitOrLoss;
            }

            var profitOrLossChartObject = new ChartObject
            {
                BackgroundColor = ConsoleColor.Blue.ToString(),
                Data = profitOrLossArray,
                Label = "Difference"

            };


            //create new List of ChartsObjects to collect Data
            var expenseDatasets = new List<ChartObject>();
            var incomeDatasets = new List<ChartObject>();
            var profitLossDatasets = new List<ChartObject> {
                totalIncomeChartObject,
                totalExpenseChartObject,
                profitOrLossChartObject
                
            };

            //create ChartObjects for expense Accounts (But I create as months)
            //I want to display each account value to stay close to each other (for better comparison).
            foreach (var group in groupedExpenseReports)
            {
                var chartObject = new ChartObject
                {
                    //I use the first object in group to find the date
                    Label = GetMonthName(group.FirstOrDefault().Month) + " " + group.FirstOrDefault().Year,
                    //Collect all the account values in that month and out them in list
                    Data = group.Select(report => report.Value).ToArray(),
                    //Create a random color, because the color cant hard written, the data can change.
                    BackgroundColor = "rgba" + (r.Next(0, 256), r.Next(0, 256), r.Next(0, 256), 1).ToString()
                };
                expenseDatasets.Add(chartObject);
            }

            //The same for Income value
            foreach (var group in groupedIncomeReports)
            {
                var chartObject = new ChartObject
                {
                    //I use the first object in group to find the date
                    Label = GetMonthName(group.FirstOrDefault().Month) + " " + group.FirstOrDefault().Year,
                    //Collect all the account values in that month and out them in list
                    Data = group.Select(report => report.Value).ToArray(),
                    //Create a random color, because the color cant hard written, the data can change.
                    BackgroundColor = "rgba" + (r.Next(0, 256), r.Next(0, 256), r.Next(0, 256), 1).ToString()
                };
                incomeDatasets.Add(chartObject);
            }

            

            var expenseChartViewModel = new ChartVM
            {
                //I use the first group to find all the names of Accounts in the group
                Labels = groupedExpenseReports.FirstOrDefault().Select(x => x.Account.Name).ToArray(),
                Datasets = expenseDatasets.ToArray()
            };

            var incomeChartViewModel = new ChartVM
            {
                
                Labels = groupedIncomeReports.FirstOrDefault().Select(x => x.Account.Name).ToArray(),
                Datasets = incomeDatasets.ToArray()
            };
            var profitLossChartVM = new ChartVM
            {
                Labels = groupedExpenseReports.Select(x => GetMonthName(x.FirstOrDefault().Month) + " " + x.FirstOrDefault().Year).ToArray(),
                Datasets = profitLossDatasets.ToArray()
            };

            var multipleChartVM = new MultipleChartsViewModel
            {
                ExpenseChartVM = expenseChartViewModel,
                IncomeChartVM = incomeChartViewModel,
                ProfitLossChart = profitLossChartVM
                
            };


            return View(multipleChartVM);
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
