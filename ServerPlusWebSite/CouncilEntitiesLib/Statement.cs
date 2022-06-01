using System;

namespace Manex.Shared
{
    public class Statement
    {
        public int StatementID { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string FilePath { get; set; }
        public string Month { get; set; }
        public bool published { get; set; }
	public bool isAvailable { get; set; }

        //FKey Rltnships
        public int? AccountID { get; set; }
        public virtual Account Account { get; set; }
    }
}