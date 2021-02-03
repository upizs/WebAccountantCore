using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public double Value { get; set; }
        public int AccountId { get; set; }

        [ForeignKey(nameof(AccountId))]
        public Account Account { get; set; }
    }

    public class ListBalanceReportVM
    {
        public List<BalanceReportVM> BalanceReports { get; set; }
        public List<ArchiveEntry> Archives { get; set; }
    }
}
