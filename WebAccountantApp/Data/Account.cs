using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAccountantApp.Data
{
    public partial class Account
    {
        

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public int AccountType { get; set; }

        
    }
}
