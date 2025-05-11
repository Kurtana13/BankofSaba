using BankofSaba.API.Models.ViewModels;
using BankofSaba.API.Models;

namespace BankofSaba.API.Services
{
    public interface IAccountService
    {
        Task<Account?> CreateAccountAsync(string username, AccountViewModel viewModel);
        Task<IEnumerable<Account>> GetAccountsAsync(string username);
        Task<Account?> DeleteAccountAsync(string username, string accountNumber);
        Task<TransactionViewModel?> CreateTransactionAsync(TransactionViewModel transactionViewModel);
    }
}
