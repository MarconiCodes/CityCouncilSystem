using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CouncilMvc.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Manex.Shared;
using CouncilMvc.ServicesX;

namespace CouncilMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private Council db;
        private ChatService chatService;

        public HomeController(ILogger<HomeController> logger, Council injectedContext)
        {
            _logger = logger;
            db = injectedContext;
            chatService = new ChatService();
        }

        public IActionResult Index()
        {
            if(User.Identity.IsAuthenticated)
            {
                return Redirect("/Home/Dashboard");
            }
            else
            {
                return View();
            }
            
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public IActionResult DashBoard()
        {
            var model = new HomeDashBoardViewModel
            {
                Accounts = db.Accounts.Include(a => a.Messages.OrderBy(m => m.MessageID)),
                Payments = db.Payments
            };

            return View(model);
        }

        [Authorize]
        public IActionResult ChatDetail(int? id)
        {
            if(!id.HasValue)
            {
                return NotFound("Http 400 Bad Request!");
            }
            var model = new HomeChatDetailViewModel
            {
                Messages = db.Messages.Where(m => m.AccountID == id).Include(x => x.Account)
            };

            if(model == null)
            {
                return NotFound($"Http 404! Product with ID {id} not found!");
            }
            return View(model);
        }

        [Authorize]
        public IActionResult SendPrivateMessage(Message message) //string id, string userInput, string message
        {
            var accID = message.AccountID;
            Account account = db.Accounts.Include(a => a.User).Where(a => a.AccountID == accID).SingleOrDefault();
            string connectionID = account.User.ConnectionID;

            message.DateSent = DateTime.Now.ToShortTimeString();
            message.FromUser = false;

            db.Messages.Add(message);
            db.SaveChanges();
            if(connectionID == null)
            { return Redirect($"/Home/ChatDetail/{accID}");}

            chatService.SendPrivateMessage("Council", message.Content, connectionID);
            
            return Redirect($"/Home/ChatDetail/{accID}");
        }
    }
}
