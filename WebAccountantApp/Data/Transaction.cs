using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAccountantApp.Data
{
    public partial class Transaction
    {
        [Key]
        public int Id { get; set; }
        public int? DebitId { get; set; }
        public int? CreditId { get; set; }
        public double Value { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Date { get; set; }

        [ForeignKey(nameof(CreditId))]
        public virtual Account Credit { get; set; }
        [ForeignKey(nameof(DebitId))]
        public virtual Account Debit { get; set; }
    }
}
