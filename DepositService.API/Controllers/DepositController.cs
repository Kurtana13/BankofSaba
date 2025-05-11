using DepositService.API.Data;
using DepositService.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DepositService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepositController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DepositController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IEnumerable<string>> GetAllAsync(string userId)
        {
            var deposits = await _context.Deposits
                .Where(d => d.UserId == userId)
                .Select(d => d.AccountId)
                .ToListAsync();
            return deposits;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<Deposit> CreateAsync(
            string name,
            string userId,
            string accountId)
        {
            Deposit deposit = new Deposit { AccountId = accountId, Name = name, UserId = userId };
            await _context.Deposits.AddAsync(deposit);
            await _context.SaveChangesAsync();
            return deposit;
        }
    }
}
