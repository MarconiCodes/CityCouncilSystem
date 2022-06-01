using System.Collections.Generic;
using Manex.Shared;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CouncilMvc.Models
{
    public class HomeDashBoardViewModel
    {
        //public IList<Account> Accounts { get; set; }
        public IQueryable<Payment> Payments { get; set; }
        public IQueryable<Account> Accounts { get; set; }
    }
}