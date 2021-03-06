﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAccountantApp.Data;

namespace WebAccountantApp.Contracts
{
    public interface ITransactionRepository : IBaseRepository<Transaction>
    {
        Task<IList<Transaction>> FilterTransacations(DateTime date);
        Task<IList<Transaction>> GetMontlyTransactions(int month, int year);
        
    }
}
