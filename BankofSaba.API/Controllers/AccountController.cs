using BankofSaba.API.Models;
using BankofSaba.API.Models.ViewModels;
using BankofSaba.API.Repositories;
using BankofSaba.API.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankofSaba.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;

        public AccountController(IAccountRepository accountRepository, IUserRepository userRepository)
        {
            _accountRepository = accountRepository;
            _userRepository = userRepository;
        }

        [Route("[action]/{username}")]
        [Authorize(Policy = "SelfOrAdmin")]
        [HttpGet]
        public async Task<ActionResult<Account>> GetAllAsync([FromRoute] string username)
        {
            try
            {
                var userResult = await _userRepository.GetByUsernameAsync(username);
                if (userResult == null)
                {
                    return NotFound($"User with username '{username}' was not found.");
                }
                var accountResult = await _accountRepository.GetAccountsByUserIdAysnc(userResult.Id);
                if (accountResult == null)
                {
                    return NotFound($"User doesn't have accounts");
                }
                return Ok(accountResult);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Route("[action]/{username}")]
        [Authorize(Policy = "SelfOrAdmin")]
        [HttpPost]
        public async Task<ActionResult<Account>> CreateAsync([FromRoute]string username, [FromBody]AccountViewModel accountViewModel)
        {
            try
            {
                var userResult = await _userRepository.GetByUsernameAsync(username);
                if (userResult == null)
                {
                    return NotFound($"User with username '{username}' was not found.");
                }
                var createdAccount = await _accountRepository.CreateAsync(userResult, accountViewModel);
                await _accountRepository.SaveAsync();
                return Ok(createdAccount);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("[action]/{username}/{accountNumber}")]
        [Authorize(Policy = "SelfOrAdmin")]
        [HttpDelete]
        public async Task<ActionResult<Account>> DeleteAsync([FromRoute]string username, [FromRoute]string accountNumber)
        {
            try
            {
                var userResult = await _userRepository.GetByUsernameAsync(username);
                var accountResult = await _accountRepository.GetByAccountNumberAsync(accountNumber);
                if (userResult == null)
                {
                    return NotFound($"User with username '{username}' was not found.");

                }
                if (accountResult == null) {
                    return NotFound($"Account '{accountNumber}' for username '{username}' was not found.");
                }
                _accountRepository.Delete(accountResult);
                await _accountRepository.SaveAsync(); 
                return Ok(accountResult);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("[action]")]
        [Authorize(Policy = "SelfOrAdmin")]
        [HttpPost]
        public async Task<ActionResult<TransactionViewModel>> CreateTransaction([FromBody]TransactionViewModel transactionViewModel)
        {
            try
            {
                var senderResult = await _userRepository.GetByUsernameAsync(transactionViewModel.Sender);
                var receiverResult = await _userRepository.GetByUsernameAsync(transactionViewModel.Receiver);
                if (senderResult == null)
                {
                    return NotFound($"User with username '{senderResult}' was not found.");

                }
                if (receiverResult == null)
                {
                    return NotFound($"User with username '{receiverResult}' was not found.");

                }

                var senderAccountResult = (await _accountRepository
                    .GetAccountsByUserIdAysnc(senderResult.Id))
                    .FirstOrDefault(a => a.Balance >= transactionViewModel.Amount);
                if (senderAccountResult == null)
                {
                    return NotFound($"User doesn't have accounts with enough funds");
                }

                var receiverAccountResult = (await _accountRepository
                    .GetAccountsByUserIdAysnc(receiverResult.Id))
                    .FirstOrDefault();
                if (receiverAccountResult == null)
                {
                    return NotFound($"User doesn't have accounts with enough funds");
                }

                senderAccountResult.Balance -= transactionViewModel.Amount;
                receiverAccountResult.Balance += transactionViewModel.Amount;

                await _accountRepository.SaveAsync();

                return Ok(transactionViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
