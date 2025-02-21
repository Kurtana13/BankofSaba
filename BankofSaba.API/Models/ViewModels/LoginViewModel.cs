using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BankofSaba.API.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [DisplayName("Username")]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
