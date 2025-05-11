using BankofSaba.API.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankofSaba.API.Models
{
    [Index(nameof(AccountNumber), IsUnique =true)]
    public class Account
    {
        public Account() { }

        public Account(string accountName,string accountNumber)
        {
            this.AccountName = accountName;
            this.AccountNumber = accountNumber;
        }

        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string AccountNumber { get; set; } = null!;
        public string? AccountName { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; } = 0;
        public string Currency { get; set; } = "GEL";
        public string IsActive { get; set; } = "A";

        public string UserId { get; set; }
        public User User { get; set; } = null!; 

        public static async Task<Account> CreateAsync(string accountName, ApplicationDbContext context)
        {
            var accountNumber = await GenerateUniqueAccountNumberAsync(context);
            return new Account (accountName, accountNumber);
        }

        private static async Task<string> GenerateUniqueAccountNumberAsync(ApplicationDbContext _context)
        {
            string newAccountNumber;
            bool exists;

            do
            {
                newAccountNumber = GenerateRandomAccountNumber();
                exists = await _context.Accounts.AnyAsync(a => a.AccountNumber == newAccountNumber);
            }
            while (exists);
            return newAccountNumber;
        }

        private static string GenerateRandomAccountNumber()
        {
            var random = new Random();
            return "AC" + random.Next(1000000000, int.MaxValue).ToString();
        }

    }
}
