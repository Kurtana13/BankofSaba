using BankofSaba.API.Models;
using BankofSaba.API.Models.ViewModels;
using BankofSaba.API.Repositories.IRepositories;
using BankofSaba.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankofSaba.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepositController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDepositService _depositService;
        private readonly IAccountService _accountService;

        public DepositController(IAccountRepository accountRepository, IUserRepository userRepository,IDepositService depositService,IAccountService accountService)
        {
            _accountRepository = accountRepository;
            _userRepository = userRepository;
            _depositService = depositService;
            _accountService = accountService;
        }

        
        [Route("[action]/{username}")]
        [Authorize(Policy = "SelfOrAdmin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAllAsync([FromRoute]string username)
        {
            try
            {
                var userResult = await _userRepository.GetByUsernameAsync(username);
                if (userResult == null)
                {
                    return NotFound($"User with username '{username}' was not found.");
                }
                var depositAccountIds = await _depositService.GetAllAsync(userResult.Id);

                var accounts = await _accountRepository.GetAllAsync(
                    a => depositAccountIds.Contains(a.Id)
                );

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
        public async Task<ActionResult<Account>> CreateAsync([FromRoute] string username, [FromBody] AccountViewModel accountViewModel)
        {
            try
            {
                var result = await _accountService.CreateAccountAsync(username, accountViewModel);
                if (result == null)
                    return NotFound($"User with username '{username}' was not found.");

                await _depositService.CreateAsync(
                    name: accountViewModel.AccountName,
                    userId: result.UserId,
                    accountId: result.Id
                );

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
