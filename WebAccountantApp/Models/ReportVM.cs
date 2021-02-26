using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using WebAccountantApp.Data;

namespace WebAccountantApp.Models
{
    
    
    public class ReportVM
    {
        public int Id { get; set; }

        public Account Account { get; set; }
        public decimal Value { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }

    //A view model for a whole report page
    public class ListReportVM
    {
        public IEnumerable<ReportVM> IncomeReports { get; set; }
        public IEnumerable<ReportVM> ExpenseReports { get; set; }
        public IEnumerable<ArchiveEntry> Archives { get; set; }
        public IEnumerable<Account> Accounts { get; set; }

        [DisplayName("Total Income")]
        public decimal IncomeSum
        {
            get
            {
                return this.IncomeReports.Select(x => x.Value).Sum();
            }
        }
        [DisplayName("Total Expense")]
        public decimal ExpenseSum
        {
            get
            {
                return this.ExpenseReports.Select(x => x.Value).Sum();
            }
        }
        //Whats left after taking all the expenses from same period income
        [DisplayName("Difference")]
        public decimal Diference
        {
            get
            {
                //Rounded because sometimes result appears to be having more than two decimal places
                return this.IncomeSum - this.ExpenseSum;
            }
        }
    }
}
