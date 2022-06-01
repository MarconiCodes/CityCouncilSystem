using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace CouncilApp.Models
{
    public class User
    {
        [PrimaryKey]
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Sex { get; set; }
        public string Password { get; set; }
        public string ConnectionID { get; set; }
        public bool IsRegistered { get; set; }

        //Foreign key relationship with the Account Table
        public int? AccountID { get; set; }
        //public virtual Account Account { get; set; }
    }
}
