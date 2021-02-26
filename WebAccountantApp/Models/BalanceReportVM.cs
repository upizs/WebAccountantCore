using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WebAccountantApp.Data;

namespace WebAccountantApp.Models
{
    public class BalanceReportVM
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
        public int AccountId { get; set; }

        [ForeignKey(nameof(AccountId))]
        public Account Account { get; set; }
    }

    public class ListBalanceReportVM
    {
        public List<BalanceReportVM> DebitBalanceReports { get; set; }
        [Display(Name ="Total")]
        public decimal TotalDebitValue
        {
            get
            {
                return this.DebitBalanceReports.Select(x => x.Value).Sum();
            }
        }
        public List<BalanceReportVM> CreditBalanceReports { get; set; }
        [Display(Name = "Total")]
        public decimal TotalCreditValue
        {
            get
            {
                return this.CreditBalanceReports.Select(x => x.Value).Sum();
            }
        }

        public List<ArchiveEntry> Archives { get; set; }

        //Gets a month name from single BalanceReport entry, becasue couldnt access it from view
        public string MonthName
        {
            get
            {
                return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(this.DebitBalanceReports.FirstOrDefault().Date.Month);
            }
        }
    }

}
