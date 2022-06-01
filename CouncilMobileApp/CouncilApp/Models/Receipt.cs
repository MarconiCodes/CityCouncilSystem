using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace CouncilApp.Models
{
    public class Receipt
    {
        [PrimaryKey]
        public int ReceiptNumber { get; set; }
        public string PaymentMethod { get; set; }
        public double AmountPaid { get; set; }
        public double RemainingBalance { get; set; }
        public string Date { get; set; }
        public int AccountID { get; set; }
    }
}
