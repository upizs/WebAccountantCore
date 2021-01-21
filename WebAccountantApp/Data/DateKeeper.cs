using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAccountantApp.Data
{
    //DateKeeper is there to record and keep the date of the last visit
    //The date of the visit at the start of the app is being compared with the last entry
    //if the month has changed BalanceReport is being recorded. Meaning the balance of the accounts at the start of a new month.
    public partial class DateKeeper
    {
        [Key]
        public int Id { get; set; }
        public DateTime? LastStarted { get; set; }
    }
}
