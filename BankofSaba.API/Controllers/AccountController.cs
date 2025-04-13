using BankofSaba.API.Models;
using BankofSaba.API.Models.ViewModels;
using BankofSaba.API.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankofSaba.API.Controllers
{
    [Route("api/[controller]")]
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
    }
}
