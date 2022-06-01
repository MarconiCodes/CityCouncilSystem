using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using SQLite;
using CouncilApp.Models;

namespace CouncilApp.Services.Core
{
    public static class DBOperator
    {
        private static SQLiteAsyncConnection db;
        public static string DatabasePath { get; set; }
        private static async Task Init()
        {
            if (db != null)
            {
                return;
            }
            var databasePath = Path.Combine(FileSystem.AppDataDirectory, $"{DatabasePath}.db");
            db = new SQLiteAsyncConnection(databasePath);

            await db.CreateTablesAsync<Account, User, Payment, Statement, Message>();
        }

        public static async Task AddAccount(Account account)
        {
            await Init();
            if (await db.Table<Account>().Where(a => a.AccountID == account.AccountID).FirstOrDefaultAsync() == null)
            {
                await db.InsertAsync(account);
            }
            
        }

        public static async Task<Account> GetAccountAsync()
        {
            await Init();
            Account account = await db.Table<Account>().FirstOrDefaultAsync();

            return account;
        }


        public static async Task AddUser(User user)
        {
            await Init();
            if (await db.Table<User>().Where(u => u.UserID == user.UserID).FirstOrDefaultAsync() == null)
            {
                await db.InsertAsync(user);
            }
        }

        public static async Task<User> GetUserAsync()
        {
            await Init();
            User user = await db.Table<User>().FirstOrDefaultAsync();
            
            return user;
        }

        public static async Task AddMessage(Message message)
        {
            await Init();
            await db.InsertAsync(message);
        }

        public static async Task<List<Message>> GetMessagesAsync()
        {
            await Init();
            var messages = await db.Table<Message>().ToListAsync();

            return messages;
        }

        public static async Task AddStatement(Statement statement)
        {
            await Init();
            await db.InsertAsync(statement);
        }

        public static async Task<List<Statement>> GetStatementsAsync()
        {
            await Init();
            var statements = await db.Table<Statement>().ToListAsync();

            return statements;
        }

        public static async Task AddPaymnent(Payment payment)
        {
            await Init();
            await db.InsertAsync(payment);
        }

        public static async Task<List<Payment>> GetPaymentsAsync()
        {
            await Init();
            var payments = await db.Table<Payment>().ToListAsync();

            return payments;
        }

        public static async Task UpdateBalance(decimal amount)
        {
            await Init();
            Account account = await db.Table<Account>().FirstOrDefaultAsync();
            account.CurrentBalance = amount;

            await db.UpdateAsync(account);
        }

        public static async Task<decimal> GetBalanceAsync()
        {
            await Init();
            Account account = await db.Table<Account>().FirstOrDefaultAsync();
            return account.CurrentBalance;
        }

        public static async Task<Statement> GetStatementAsync(int? id)
        {
            await Init();
            Statement statement = await db.Table<Statement>().Where(s => s.StatementID == id).FirstAsync();

            return statement;
        }

        public static async Task UpdateStatement(Statement UpdatedStatement)
        {
            await Init();

            await db.UpdateAsync(UpdatedStatement);
        }

        public static void ResetDB()
        {
            db = null;
        }
    }
}
