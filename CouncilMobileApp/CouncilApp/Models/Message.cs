using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace CouncilApp.Models
{
    public class Message
    {
        
        [PrimaryKey, AutoIncrement]
        public int MessageID { get; set; }
        public bool FromUser { get; set; }
        public string Sender { get; set; }
        public string Content { get; set; }
        public string DateSent { get; set; }
    }
}
