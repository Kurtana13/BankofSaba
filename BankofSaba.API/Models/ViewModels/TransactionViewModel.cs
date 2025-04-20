using System.ComponentModel.DataAnnotations;

namespace BankofSaba.API.Models.ViewModels
{
    public class TransactionViewModel
    {
        [Required]
        public string Sender { get; set; }

        [Required]
        public string Receiver { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }
    }
}
