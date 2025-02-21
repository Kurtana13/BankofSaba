using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BankofSaba.API.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [DisplayName("Username")]
        public string Username { get; set; } = null!;
        [Required]
        [EmailAddress]
        [DisplayName("Email")]
        public string? Email { get; set; }
        [Required]
        [DisplayName("Password")]
        public string? Password { get; set; }
        [Required]
        [DisplayName("Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords don't match!")]
        public required string? ConfirmPassword { get; set; }

    }
}
