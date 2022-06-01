using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Manex.Shared;

namespace CouncilMvc.Hubs
{
    public class ChatHub : Hub
    {
        private Council db;

        public ChatHub(Council injectedContext)
        {
            db = injectedContext;
        }
        public async Task SendMessage(string sender, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", sender, message);
        }

        public async Task SendPrivateMessage(string sender, string message, string connectionID)
        {
            await Clients.Client(connectionID).SendAsync("ReceiveMessage", sender, message);
        }

        public async Task SendPrivateTrigger(string connectionID)
        {
            await Clients.Client(connectionID).SendAsync("UpdateReceipts");
        }

        public async Task SendMessageToCouncil(string accountNumber, string message)
        {
            Account account = db.Accounts.Where(a => a.AccountNumber == accountNumber).SingleOrDefault();
            if (account != null)
            {
                Message newMessage = new Message
                {
                    AccountID = account.AccountID,
                    FromUser = true,
                    DateSent = DateTime.Now.ToShortTimeString(),
                    Content = message
                };

                await db.Messages.AddAsync(newMessage);
                db.SaveChanges();
            }
        }

        public async Task PublishBill(Statement statement, string connectionID)
        {
            string fileName = statement.FileName;
            string fileUrl = statement.FileUrl;
            string month = statement.Month;

            await Clients.Client(connectionID).SendAsync("ReceiveBill", fileName, fileUrl, month);
        }

        public async Task OnConnectt(string accountNumber)
        {
            string connectionID = Context.ConnectionId;
            Account account = db.Accounts.Include(a => a.Statements.Where(s => s.published == false)).Where(a => a.AccountNumber == accountNumber).SingleOrDefault();
            if (account != null)
            {
                foreach (Statement statement in account.Statements)
                {
                    await PublishBill(statement, connectionID);
                    statement.published = true;
                    db.SaveChanges();
                }
            }
        }

        public void OnRegister(string accountNumber)
        {
            Account account = db.Accounts.Include(a => a.User).FirstOrDefault(a => a.AccountNumber == accountNumber);
            string conID = Context.ConnectionId;
            if (conID != null)
            {
                account.User.ConnectionID = conID;
                db.SaveChanges();
            }
        }


        // public override Task OnConnectedAsync()
        // {

        //     return base.OnConnectedAsync();
        // }
    }
}