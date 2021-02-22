using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAccountantApp.Models
{
    public class MultipleChartsViewModel
    {
        public ChartVM ExpenseChartVM { get; set; }
        public ChartVM IncomeChartVM { get; set; }
        public ChartVM ProfitLossChart { get; set; }
    }
}
