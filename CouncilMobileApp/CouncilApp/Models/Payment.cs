using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace CouncilApp.Models
{
    public class Payment
    {
        [PrimaryKey, AutoIncrement]
        public int ReceiptNumber { get; set; }
        public string PaymentMethod { get; set; }
        public string Date { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal RemainingBalance { get; set; }


        //FKRY Rs w/ other Ts
        public int? AccountID { get; set; }
        //public virtual Account Account { get; set; }
    }
}
