using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebAccountantApp.Data;

namespace WebAccountantApp.Models
{
    public class AccountVM
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        public decimal Value { get; set; }
        [Required]
        [EnumDataType(typeof(AccountType))]
        [Display(Name = "Account Type")]
        public AccountType AccountType { get; set; }
    }

    public class CreateAccountVM
    {
        public AccountVM Account { get; set; }
        public List<AccountType> AccountTypes { get; set; }

    }
    public class AccountsVM
    {
        [Display(Name = "Expense and Income Accounts")]
        public List<AccountVM> ExpenseAndIncomeAccounts;
        [Display(Name = "Credit Accounts")]
        public List<AccountVM> CreditAccounts;
        [Display(Name = "Debit Accounts")]
        public List<AccountVM> DebitAccounts;
        [Display(Name = "Total")]
        public decimal TotalCreditValue
        {
            get
            {
                return this.CreditAccounts.Select(x => x.Value).Sum();
            }
        }
        [Display(Name = "Total")]

        public decimal TotalDebitValue
        {
            get
            {
                return this.DebitAccounts.Select(x => x.Value).Sum();
            }
        }
        

    }
}
