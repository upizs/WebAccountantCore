﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAccountantApp.Data
{
    public partial class Account
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
        public AccountType AccountType { get; set; }

        
    }
}
