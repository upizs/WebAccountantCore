﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAccountantApp.Data;

namespace WebAccountantApp.Contracts
{
    public interface IBalanceReport : IBaseRepository<BalanceReport>
    {
        Task<IList<BalanceReport>> GetBalanceReportByMonth(int month, int year);

    }
}
