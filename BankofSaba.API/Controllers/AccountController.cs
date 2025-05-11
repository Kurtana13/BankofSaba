using BankofSaba.API.Models;
using BankofSaba.API.Models.ViewModels;
using BankofSaba.API.Repositories;
using BankofSaba.API.Repositories.IRepositories;
using BankofSaba.API.Services;
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
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [Route("[action]/{username}")]
        [Authorize(Policy = "SelfOrAdmin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAllAsync(string username)
        {
            try
            {
                var accounts = await _accountService.GetAccountsAsync(username);
                if (!accounts.Any())
                    return NotFound("User doesn't have accounts or user not found.");
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("[action]/{username}")]
        [Authorize(Policy = "SelfOrAdmin")]
        [HttpPost]
        public async Task<ActionResult<Account>> CreateAsync(string username, AccountViewModel viewModel)
        {
            try
            {
                var result = await _accountService.CreateAccountAsync(username, viewModel);
                if (result == null)
                    return NotFound($"User with username '{username}' was not found.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("[action]/{username}/{accountNumber}")]
        [Authorize(Policy = "SelfOrAdmin")]
        [HttpDelete]
        public async Task<ActionResult<Account>> DeleteAsync(string username, string accountNumber)
        {
            try
            {
                var result = await _accountService.DeleteAccountAsync(username, accountNumber);
                if (result == null)
                    return NotFound($"User or account not found.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("[action]")]
        [Authorize(Policy = "SelfOrAdmin")]
        [HttpPost]
        public async Task<ActionResult<TransactionViewModel>> CreateTransaction(TransactionViewModel transaction)
        {
            try
            {
                var result = await _accountService.CreateTransactionAsync(transaction);
                if (result == null)
                    return NotFound("Sender/receiver invalid or insufficient funds.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}
