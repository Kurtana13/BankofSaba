using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BankofSaba.API.Models.ViewModels
{
    public class AccountViewModel
    {
        [DisplayName("Account Name")]
        public string ?AccountName { get; set; }
    }
}
