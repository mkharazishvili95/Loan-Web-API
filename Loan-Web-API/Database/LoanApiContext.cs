using Loan_Web_API.Identity;
using Loan_Web_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Loan_Web_API.Database
{
    public class LoanApiContext : DbContext
    {
        public LoanApiContext(DbContextOptions<LoanApiContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Loggs> Loggs { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<BlockedUsers> BlockedUsers {  get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Bank>().HasData(
              new Bank
              {
                  Id = 1,
                  GEL_Balance = 1000000,
                  USD_Balance = 2000000,
                  EUR_Balance = 3000000
              });
        }
    }
}
