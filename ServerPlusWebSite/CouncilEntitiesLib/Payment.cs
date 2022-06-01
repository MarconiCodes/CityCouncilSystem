using System;

namespace Manex.Shared
{
    public class Payment
    {
        public int PaymentID { get; set; }
        public string PaymentMethod { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal RemainingBalance { get; set; }
        public string Date { get; set; }


        //FKRY Rs w/ other Ts
        public int? AccountID { get; set; }
        public virtual Account Account { get; set; }
    }
}