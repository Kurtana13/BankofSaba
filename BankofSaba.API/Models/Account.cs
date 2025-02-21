using System.ComponentModel.DataAnnotations.Schema;

namespace BankofSaba.API.Models
{
    public class Account
    {
        public string Id { get; set; }
        public string AccountNumber { get; set; } = null!;
        public string? AccountName { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; } = 0;
        public string Currency { get; set; } = "GEL";
        public string IsActive { get; set; } = "A";

        public string UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
