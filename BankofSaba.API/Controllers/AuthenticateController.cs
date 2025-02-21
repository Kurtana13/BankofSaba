using BankofSaba.API.Infrastructure;
using BankofSaba.API.Models;
using BankofSaba.API.Models.ViewModels;
using BankofSaba.API.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BankofSaba.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly TokenProvider tokenProvider;
        private readonly IUserRepository userRepository;

        public AuthenticateController(UserManager<User> userManager, TokenProvider tokenProvider, IUserRepository userRepository)
        {
            this.userManager = userManager;
            this.tokenProvider = tokenProvider;
            this.userRepository = userRepository;
        }

        [Route("[action]")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await userManager.FindByNameAsync(model.Username);
            if (user == null) 
                return Unauthorized("Invalid username or password");

            var isPasswordValid = await userManager.CheckPasswordAsync(user, model.Password);
            if (!isPasswordValid)
                return Unauthorized("Invalid username or password");

            var token = await tokenProvider.CreateAsync(user);

            return Ok(new { Token = token });
        }
    }
}
