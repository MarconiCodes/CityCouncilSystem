using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace CouncilMvc.ServicesX
{
    public class ChatService
    {
        HubConnection connection;
        public ChatService()
        {
            //connection = new HubConnectionBuilder().WithUrl("https://172.20.10.7:5001/ChatHub").Build();
            connection = new HubConnectionBuilder().WithUrl("https://172.20.10.6:5001/ChatHub", (opts) =>
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

            Connect();
            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };
        }

        public async void Connect()
        {
            try
            {
                await connection.StartAsync();
            }
            catch (System.Exception) { throw; }
        }

        public async void SendMessage(string sender, string message)
        {
            try
            {
                await connection.InvokeAsync("sendMessage", sender, message);
            }
            catch (System.Exception) { throw; }
        }

        public async void SendPrivateMessage(string sender, string message, string connectionID)
        {
            try
            {
                await connection.InvokeAsync("sendPrivateMessage", sender, message, connectionID);
            }
            catch (System.Exception){ throw; }
        }

        public async void SendPrivateTrigger(string connectionID)
        {
            try
            {
                await connection.InvokeAsync("SendPrivateTrigger", connectionID);
            }
            catch(System.Exception){ throw; }
        }
    }
}