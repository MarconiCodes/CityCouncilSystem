using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SignalRServer.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string userid, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", userid, message);
        }
    }
}