using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace CouncilApp.Models
{
    public class Statement
    {
        [PrimaryKey, AutoIncrement]
        public int StatementID { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string Month { get; set; }
        public bool Published { get; set; }
        public string DateSent { get; set; }
        public string Timesent { get; set; }
        public bool IsNotDownloaded { get; set; }

        //FKey Rltnships
        public int? AccountID { get; set; }
        //public virtual Account Account { get; set; }
    }
}
