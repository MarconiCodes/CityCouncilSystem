using System;
using System.Collections.Generic;

namespace Manex.Shared
{
    public class Account
    {
        public int AccountID { get; set; }
        public string AccountNumber { get; set; }
        public decimal CurrentBalance { get; set; }

        // foreign key relationships with other tables
        public ICollection<Message> Messages { get; set; }
        public ICollection<Statement> Statements { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public virtual User User { get; set; }

        public Account()
        {
            this.Messages = new List<Message>();
            this.Statements = new List<Statement>();
            this.Payments = new List<Payment>();
        }
    }
}
