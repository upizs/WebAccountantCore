using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebAccountantApp.Data;

namespace WebAccountantApp.Models
{
    public class TransactionVM
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int? DebitId { get; set; }
        [Required]
        public int? CreditId { get; set; }
        [Required]
        public double Value { get; set; }
        
        public DateTime Date { get; set; }

        [ForeignKey(nameof(CreditId))]
        public Account Credit { get; set; }
        [ForeignKey(nameof(DebitId))]
        public Account Debit { get; set; }
    }

    public class CreateTransactionVM
    {
        public TransactionVM Transaction { get; set; }
        public List<AccountVM> Accounts { get; set; }
    }
}
