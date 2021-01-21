using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAccountantApp.Data
{
    public partial class DateKeeper
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastStarted { get; set; }
    }
}
