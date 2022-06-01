using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Webdev.Payments;
using Webdev.Core;
using Manex.Shared;
using CouncilMvc.ServicesX;


namespace CouncilMvc.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServicesController : ControllerBase
    {
        private readonly ILogger<ServicesController> _logger;
        private Council db;
        private Paynow paynow;
        private ChatService chatService;
        private System.Timers.Timer timer;
        private Dictionary<int, string> monthDict;

        public ServicesController(ILogger<ServicesController> logger, Council injectedContext)
        {
            _logger = logger;
            db = injectedContext;

            timer = new System.Timers.Timer();
            timer.Interval = 60 * 1000;           // 24 * 60 * 60 * 1000;
            //timer.Elapsed += RetrieveFiles;
            //timer.Start();

            monthDict = new Dictionary<int, string>{
                {1 , "January"},
                {2 , "February"},
                {3 , "March"},
                {4 , "April"},
                {5 , "May"},
                {6 , "June"},
                {7 , "July"},
                {8 , "August"},
                {9 , "September"},
                {10 , "October"},
                {11 , "November"},
                {12 , "December"}
            };

            //CreateStatementsRecords();

            chatService = new ChatService();
        }

        //Get /Services/AccountNumber/firstName/lastName/email/phone
        [HttpGet("{AccountNumber}/{firstName}/{lastName}/{email}/{phone}/{password}")]
        public Account GetAccount(string AccountNumber, string firstName, string lastName, string email, string phone, string password)
        {
            Account account = db.Accounts.Include(a => a.User).FirstOrDefault(a => a.AccountNumber == AccountNumber);
            if (account != null)
            {
                if (account.User.FirstName == firstName && account.User.LastName == lastName)
                {
                    account.User.IsRegistered = true;
                    account.User.Email = email;
                    account.User.Phone = phone;
                    account.User.Password = password;

                    db.SaveChanges();

                    return account;
                }
            }

            return null;
        }

        //Get /Services/accountNumber/email/method/phone/amount
        [HttpGet("{accountNumber}/{email}/{method}/{phone}/{amount}")]
        public Manex.Shared.Payment MakePayment(string accountNumber, string email, string method, string phone, decimal amount)
        {
            Account account = db.Accounts.SingleOrDefault(a => a.AccountNumber == accountNumber);
            User user = db.Users.SingleOrDefault(u => u.AccountID == account.AccountID);
            Manex.Shared.Payment receipt = new Manex.Shared.Payment();

            paynow = new Paynow("13552", "a1e6a451-61f4-49b0-ad7f-48b5b2772785");
            var payment = paynow.CreatePayment("Test Transaction", email);
            payment.Add(accountNumber, amount);
            string pollurl;
            StatusResponse status;

            var response = paynow.SendMobile(payment, phone, method);
            if (response.Success())
            {
                pollurl = response.PollUrl();
                status = paynow.PollTransaction(pollurl);
            }
            else
            {
                var errs = response.Errors();
                System.IO.File.AppendAllText("c:\\users\\lenovo\\desktop\\API_log.txt", $"\n{errs}");
                return null;
            }
            Thread.Sleep(1000);

            // Check the status of the transaction with the specified pollUrl

            if (status.Success())
            {
                if (account != null)
                {
                    account.CurrentBalance -= amount;
                    receipt.AccountID = account.AccountID;
                    receipt.PaymentMethod = "paynow";
                    receipt.AmountPaid = amount;
                    receipt.RemainingBalance = account.CurrentBalance;
                    receipt.Date = DateTime.Now.ToLongDateString();
                    db.Payments.Add(receipt);

                    db.SaveChanges();
                    return receipt;
                }
                System.IO.File.AppendAllText("c:/users/lenovo/desktop/API_log.txt", "\nAccount not found!");
                return null;
            }
            else
            {
                System.IO.File.AppendAllText("c:/users/lenovo/desktop/API_log.txt", "\nWhy you no pay?");
                return null;
            }

            //return "HOLAN";
        }

        //Get /Services/filePath/fileName
        [HttpGet("{filePath}/{fileName}")]
        public IActionResult DownloadFile(string filePath, string fileName)
        {
            RetrieveFiles();
            string actualFilePath = filePath.Replace('*', '\\');
            if(System.IO.File.Exists(actualFilePath)){
                var fs = new FileStream(actualFilePath, FileMode.Open);
             
                return File(fs, "application/pdf", fileName);
            }
            return new EmptyResult();
        }
        

        // private async Task<InitResponse> GetResponseAsync()
        // {
        //     Webdev.Payments.Payment payment = new Webdev.Payments.Payment("dd", "");
        //     var response = await paynow.SendMobile(payment, "", "");
        // }

        private void RetrieveFiles()
        {
            //string basePath = $@"{Directory.GetCurrentDirectory()}\..\Statements";
            string basePath = "C:\\Users\\lenovo\\Desktop\\HIT200 Project\\CouncilMvc\\..\\Statements";
            string baseUrl = $"https://172.20.10.7:5001/Services";
            int currentDay = DateTime.Now.Day;
            string currentMonth = monthDict[DateTime.Now.Month];
            List<Statement> statements;

            if(currentDay >= 20 || currentDay <= 6)
            {
                statements = db.Statements.Include(s => s.Account).Where(s => s.isAvailable == false && s.Month == currentMonth).ToList<Statement>();
                foreach (Statement statement in statements)
                {
                    string filePath = Path.Combine(basePath, currentMonth, $"{statement.Account.AccountNumber}.pdf");
                    if (System.IO.File.Exists(filePath))
                    {
                        string initFilepath = filePath.Replace('\\', '*');
                        statement.isAvailable = true;
                        statement.FileName = $"{statement.Account.AccountNumber}.pdf";
                        statement.FilePath = filePath;
                        statement.FileUrl = $"{baseUrl}/{initFilepath}/{statement.FileName}";
                        db.SaveChanges();
                    }
                }
            }
        }

        private async void CreateStatementsRecords()
        {
            List<Account> accounts= await db.Accounts.ToListAsync();
            List<Statement> statements = new List<Statement>();

            foreach (Account account in accounts)
            {
                Statement statement = new Statement{
                    AccountID = account.AccountID,
                    Month = "April",
                    published = false,
                    isAvailable = false
                };
                statements.Add(statement);
            }

            foreach(Statement statement in statements)
            {
                db.Statements.Add(statement);
                db.SaveChanges();
            }
        }

    }
}
