using BankofSaba.API.Models;

namespace DepositService.API.Models
{
    public class Deposit
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = null!;

        public string AccountId { get; set; } = null!;

        public string UserId { get; set; } = null!;
    }
}
