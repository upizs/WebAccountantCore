﻿
using System.Globalization;


namespace WebAccountantApp.Models
{
    public class ArchiveEntry
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName
        {
            get
            {
                return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(this.Month);
            }
        }
    }
}
