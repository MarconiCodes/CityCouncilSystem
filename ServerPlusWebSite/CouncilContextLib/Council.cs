using System;
using Microsoft.EntityFrameworkCore;

namespace Manex.Shared
{
    public class Council : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Statement> Statements { get; set; }
        public DbSet<Payment> Payments { get; set; }

        public Council(DbContextOptions<Council> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>().Property(a => a.AccountID)
                                          .IsRequired();
            modelBuilder.Entity<Account>().HasMany(a =>a.Messages)
                                          .WithOne(m => m.Account);
            modelBuilder.Entity<Account>().HasMany(a => a.Statements)
                                          .WithOne(s => s.Account);
            modelBuilder.Entity<Account>().HasMany(a => a.Payments)
                                          .WithOne(p => p.Account);
            modelBuilder.Entity<Account>().HasOne(a => a.User)
                                          .WithOne(u => u.Account);

            modelBuilder.Entity<Message>().HasOne(m => m.Account)
                                          .WithMany(a => a.Messages);
            
            modelBuilder.Entity<Payment>().Property(p => p.PaymentID)
                                          .IsRequired();
            modelBuilder.Entity<Payment>().HasOne(p => p.Account)
                                          .WithMany(a => a.Payments);
            
            modelBuilder.Entity<Statement>().Property(s => s.StatementID)
                                            .IsRequired();
            modelBuilder.Entity<Statement>().HasOne(s => s.Account)
                                            .WithMany(a => a.Statements);
            
            modelBuilder.Entity<User>().Property(u => u.UserID)
                                       .IsRequired();
            modelBuilder.Entity<User>().HasOne(u => u.Account)
                                       .WithOne(a => a.User);
            
        }
    }
}
