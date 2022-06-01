using System.Collections.Generic;
using Manex.Shared;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CouncilMvc.Models
{
    public class HomeChatDetailViewModel
    {
        public IQueryable<Message> Messages { get; set; }
    }
}