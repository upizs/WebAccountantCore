using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAccountantApp.Data
{
    //Type of accounts determens how account will behave in transactions and reports.
    public enum AccountType
    {
        Debit,
        Credit,
        Income,
        Expense
    }
}
