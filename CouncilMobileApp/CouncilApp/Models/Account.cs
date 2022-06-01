using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using CouncilApp.Services.Core;

namespace CouncilApp.Models
{
    public class Account
    {
        [PrimaryKey]
        public int AccountID { get; set; }
        public string AccountNumber { get; set; }
        public decimal CurrentBalance { get; set; }

        [Ignore]
        public User User { get; set; }
    }
}
