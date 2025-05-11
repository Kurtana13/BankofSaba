using BankofSaba.API.Models;
using BankofSaba.API.Models.ViewModels;
using BankofSaba.API.Repositories.IRepositories;

namespace BankofSaba.API.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository, IUserRepository userRepository)
        {
            _accountRepository = accountRepository;
            _userRepository = userRepository;
        }


        public async Task<Account?> CreateAccountAsync(string username, AccountViewModel viewModel)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null) return null;

            var account = await _accountRepository.CreateAsync(user, viewModel);
            await _accountRepository.SaveAsync();
            return account;
        }


        public async Task<IEnumerable<Account>> GetAccountsAsync(string username)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null) return Enumerable.Empty<Account>();

            return await _accountRepository.GetAccountsByUserIdAysnc(user.Id);
        }


        public async Task<Account?> DeleteAccountAsync(string username, string accountNumber)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null)
                throw new ArgumentNullException($"User with username '{username}' not found.");

            var account = await _accountRepository.GetByAccountNumberAsync(accountNumber);
            if (account == null)
                throw new ArgumentNullException($"Account with account number '{accountNumber}' not found.");

            _accountRepository.Delete(account);
            await _accountRepository.SaveAsync();
            return account;
        }


        public async Task<TransactionViewModel?> CreateTransactionAsync(TransactionViewModel transaction)
        {
            var sender = await _userRepository.GetByUsernameAsync(transaction.Sender);
            if (sender == null)
            {
                throw new ArgumentException($"Sender with username '{transaction.Sender}' was not found.");
            }

            var receiver = await _userRepository.GetByUsernameAsync(transaction.Receiver);
            if (receiver == null)
            {
                throw new ArgumentException($"Receiver with username '{transaction.Receiver}' was not found.");
            }

            var senderAccount = (await _accountRepository
                .GetAccountsByUserIdAysnc(sender.Id))
                .FirstOrDefault(a => a.Balance >= transaction.Amount);

            if (senderAccount == null)
            {
                throw new InvalidOperationException("Sender does not have enough funds for the transaction.");
            }

            var receiverAccount = (await _accountRepository
                .GetAccountsByUserIdAysnc(receiver.Id))
                .FirstOrDefault();

            if (receiverAccount == null)
            {
                throw new InvalidOperationException("Receiver does not have an account.");
            }

            senderAccount.Balance -= transaction.Amount;
            receiverAccount.Balance += transaction.Amount;

            await _accountRepository.SaveAsync();
            return transaction;
        }
    }
}
