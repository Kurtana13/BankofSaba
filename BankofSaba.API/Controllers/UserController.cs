using BankofSaba.API.Infrastructure;
using BankofSaba.API.Models;
using BankofSaba.API.Models.ViewModels;
using BankofSaba.API.Repositories;
using BankofSaba.API.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BankofSaba.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly TokenProvider _tokenProvider;

        public UserController(IUserRepository userRepository, TokenProvider tokenProvider) {
            _userRepository = userRepository;
            _tokenProvider = tokenProvider;
        }

        [Route("[action]")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<User>> CreateAsync([FromBody]RegisterViewModel registerViewModel)
        {
            if (registerViewModel == null)
            {
                return BadRequest("Object is Null");
            }

            try
            {
                var createdUser = await _userRepository.CreateAsync(registerViewModel);

                if (createdUser == null)
                {
                    return BadRequest("User Already Exists");
                }
                await _userRepository.SaveAsync();
                return Ok(await _tokenProvider.CreateAsync(createdUser));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while creating the user. Please try again later.");
            }
        }

        [Route("[action]")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            try
            {
                return Ok(await _userRepository.GetAllAsync());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data");
            }
        }


        
        [Route("[action]/{username}")]
        [Authorize(Policy = "SelfOrAdmin")]
        [HttpDelete]
        public async Task<ActionResult<User>> DeleteUser([FromRoute]string username)
        {
            try
            {
                var user = await _userRepository.GetByUsernameAsync(username);
                _userRepository.Delete(user);
                await _userRepository.SaveAsync();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}
