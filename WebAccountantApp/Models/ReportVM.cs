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
        public double Value { get; set; }
    }

    public class ListReportVM
    {
        public IEnumerable<ReportVM> IncomeReports { get; set; }
        public IEnumerable<ReportVM> ExpenseReports { get; set; }
        //public IEnumerable<ArchiveEntry> Archives { get; set; }
        public IEnumerable<Account> Accounts { get; set; }

        [DisplayName("Total Income")]
        public double IncomeSum
        {
            get
            {
                return this.IncomeReports.Select(x => x.Value).Sum();
            }
        }
        [DisplayName("Total Expense")]
        public double ExpenseSum
        {
            get
            {
                return this.ExpenseReports.Select(x => x.Value).Sum();
            }
        }
        [DisplayName("Difference")]
        public double Diference
        {
            get
            {
                //Rounded because sometimes result appears to be having more than two decimal places
                return Math.Round(this.IncomeSum - this.ExpenseSum, 2);
            }
        }
    }
}
