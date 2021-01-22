﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using WebAccountantApp.Models;

namespace WebAccountantApp.Data
{
    public partial class MyContext : DbContext
    {
        public MyContext()
        {
        }

        public MyContext(DbContextOptions<MyContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<BalanceReport> BalanceReports { get; set; }
        public virtual DbSet<DateKeeper> DateKeepers { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public DbSet<WebAccountantApp.Models.AccountVM> AccountVM { get; set; }

    }
}
