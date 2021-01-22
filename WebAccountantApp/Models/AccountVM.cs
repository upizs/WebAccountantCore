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
        public double Value { get; set; }
        [Required]
        [EnumDataType(typeof(AccountType))]
        [Display(Name = "Account Type")]
        public AccountType AccountType { get; set; }
    }
}
