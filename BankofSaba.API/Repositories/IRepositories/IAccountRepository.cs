using BankofSaba.API.Models;
using BankofSaba.API.Models.ViewModels;

namespace BankofSaba.API.Repositories.IRepositories
{
    public interface IAccountRepository:IGenericRepository<Account>
    {
        Task<Account> CreateAsync(User user, Account account);
        Task<Account> CreateAsync(User user, AccountViewModel accountViewModel);
    }
}
