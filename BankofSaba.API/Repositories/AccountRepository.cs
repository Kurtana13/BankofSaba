using BankofSaba.API.Data;
using BankofSaba.API.Models;
using BankofSaba.API.Models.ViewModels;
using BankofSaba.API.Repositories.IRepositories;

namespace BankofSaba.API.Repositories
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        public AccountRepository(ApplicationDbContext context) : base(context)
        {

        }


        public async Task<Account> CreateAsync(User user, Account account)
        {
            account.User = user;
            return await CreateAsync(account);
        }

        public async Task<Account> CreateAsync(User user, AccountViewModel accountViewModel)
        {
            var account = await Account.CreateAsync(accountViewModel.AccountName, _context);
            return await CreateAsync(user, account);
        }
    }
}
