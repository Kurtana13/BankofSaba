using BankofSaba.API.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace BankofSaba.API.Models
{
    public class User:IdentityUser
    {
        public User() { }

        public User(RegisterViewModel registerViewModel) : base()
        {
            this.UserName = registerViewModel.Username;
            this.Email = registerViewModel.Email;
        }

       
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public virtual ICollection<Account>? Accounts { get; set; }
    }
}
