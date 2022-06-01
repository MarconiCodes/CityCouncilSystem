using System;

namespace Manex.Shared
{
    public class Message
    {
        public int MessageID { get; set; }
        public bool FromUser { get; set; }
        public string DateSent { get; set; }
        public string Content { get; set; }

        // FKEY relationship with Account Table
        public int? AccountID { get; set; }
        public virtual Account Account { get; set; }
    }
}