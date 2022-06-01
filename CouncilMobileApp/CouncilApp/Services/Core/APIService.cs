using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using CouncilApp.Models;
using Newtonsoft.Json;
using CouncilApp;
using System.Net.NetworkInformation;

namespace CouncilApp.Services.Core
{
    public class APIService
    {
        private static HttpClientHandler clientHandler;
        private static HttpClient client;
        
        public static async Task<PreAccount> GetAccountAsync(string path)
        {
            Init();

            try
            {
                var response = await client.GetAsync($"https://172.20.10.7:5001/Services/{path}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    PreAccount account = JsonConvert.DeserializeObject<PreAccount>(content);
                    return account;
                }
            }
            catch(HttpRequestException)
            {
                return null;
            }
            catch(ArgumentNullException)
            {
                return null;
            }
            
            return null;
        }

        public static async Task<Payment> MakePaynowPaymentAsync(string path)
        {
            Init();

            try
            {
                var response = await client.GetAsync($"https://172.20.10.7:5001/Services/{path}"); //{accountNumber}/{email}/{method}/{phone}/{amount}

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Payment payment = JsonConvert.DeserializeObject<Payment>(content);
                    return payment;
                }
            }
            catch (HttpRequestException) { }
            catch (ArgumentNullException) { }
            catch (Exception) { }

            return null;
        }

        private static void Init()
        {
            clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
            client = new HttpClient(clientHandler);
        }

        public static bool CheckInternetConnection()
        {
            try
            {
                Ping ping = new Ping();
                string host = "8.8.8.8";
                byte[] buffer = new byte[32];
                int timeout = 1000;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = ping.Send(host, timeout, buffer, pingOptions);
                return reply.Status == IPStatus.Success;
            }
            catch (Exception) { return false; }
        }

        public static bool pingServer()
        {
            try
            {
                Ping ping = new Ping();
                string host = "172.20.10.7";
                byte[] buffer = new byte[32];
                int timeout = 1500;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = ping.Send(host, timeout, buffer, pingOptions);
                return reply.Status == IPStatus.Success;
            }
            catch (Exception) { return false; }
        }

    }
}
