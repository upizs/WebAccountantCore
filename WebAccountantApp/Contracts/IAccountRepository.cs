using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAccountantApp.Data;

namespace WebAccountantApp.Contracts
{
    public interface IAccountRepository : IBaseRepository<Account>
    {
        Task<IList<Account>> GetAccountByTwoTypes(AccountType type1, AccountType type2);
        Task<IList<Account>> GetAccountByType(AccountType type);

    }
}
