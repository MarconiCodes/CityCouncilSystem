using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.SignalR.Client;
using CouncilApp.Services.Interfaces;
using CouncilApp.Models;

namespace CouncilApp.Services.Core
{
    public class ChatService : IChartService
    {
        private readonly HubConnection hubConnection;
        private Account account;

        public ChatService(Account acc)
        {
            account = acc;
            //hubConnection = new HubConnectionBuilder().WithUrl("https://172.20.10.7:5001/ChatHub").Build();
            hubConnection = new HubConnectionBuilder().WithUrl("https://172.20.10.7:5001/ChatHub", (opts) =>
            {
                opts.HttpMessageHandlerFactory = (message) =>
                {
                    if (message is HttpClientHandler clientHandler)
                        // always verify the SSL certificate
                        clientHandler.ServerCertificateCustomValidationCallback +=
                            (sender, certificate, chain, sslPolicyErrors) => { return true; };
                    return message;
                };
            }).Build();
 
            try
            {
                Connect();
            }
            catch (HttpRequestException) { }
            catch(Exception) { }
            
        }

        public async void Connect()
        {
            await hubConnection.StartAsync();
            await Register(account.AccountNumber);
            await Connect(account.AccountNumber);
        }

        public async Task Disconnect()
        {
            await hubConnection.StopAsync();
        }

        public async Task sendMessage(string userid, string message)
        {
            await hubConnection.InvokeAsync("SendMessage", userid, message);
        }

        public async Task SendMessageToCouncil(string accountNumber, string message)
        {
            await hubConnection.InvokeAsync("SendMessageToCouncil", accountNumber, message);
        }

        public void ReceiveMessage(Action<string, string> GetMessageAndUser)
        {
            hubConnection.On("ReceiveMessage", GetMessageAndUser);
        }

        public void ReceiveBill(Action<string, string, string> GetBill)
        {
            hubConnection.On("ReceiveBill", GetBill);
        }

        public void UpdateReceipts(Action UpdateStaff)
        {
            hubConnection.On("UpdateReceipts", UpdateStaff);
        }

        public async Task Register(string accountNumber)
        {
            await hubConnection.InvokeAsync("OnRegister", accountNumber);
        }

        public async Task Connect(string accountNumber)
        {
            await hubConnection.InvokeAsync("OnConnectt", accountNumber);
        }

        public void Dispose()
        {

        }
    }
}
