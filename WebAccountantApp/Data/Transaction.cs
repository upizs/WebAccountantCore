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
        [Required]
        public int? DebitId { get; set; }
        [Required]
        public int? CreditId { get; set; }
        [Required]
        [Column(TypeName = "decimal(20,2)")]
        public decimal Value { get; set; }
        [Required]
        public DateTime Date { get; set; }

        [ForeignKey(nameof(CreditId))]
        public Account Credit { get; set; }
        [ForeignKey(nameof(DebitId))]
        public Account Debit { get; set; }
    }
}
