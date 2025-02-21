using BankofSaba.API.Data;
using BankofSaba.API.Models;
using BankofSaba.API.Models.ViewModels;
using BankofSaba.API.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BankofSaba.API.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly UserManager<User> _userManager;

        public UserRepository(ApplicationDbContext context, UserManager<User> userManager)
            : base(context)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        private async Task<User> CreateAsync(User user, string password)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Check if username already exists
                    if (await _dbSet.AnyAsync(x => x.UserName == user.UserName))
                    {
                        return null;
                    }

                    var createdUser = await _userManager.CreateAsync(user, password);

                    if (createdUser.Succeeded)
                    {
                        var roleResult = await _userManager.AddToRoleAsync(user, "User");

                        if (roleResult.Succeeded)
                        {
                            await transaction.CommitAsync(); // Commit the transaction
                            return user; // Return created user
                        }
                        else
                        {
                            await transaction.RollbackAsync(); // Rollback if role assignment fails
                        }
                    }
                    
                }
                catch (Exception ex)
                {
                    // Log or handle the exception as needed
                    await transaction.RollbackAsync(); // Rollback on error
                }
            }

            return null; // Return null if user creation fails
        }

        public async Task<User> CreateAsync(RegisterViewModel registerViewModel)
        {
            return await CreateAsync(new User(registerViewModel), registerViewModel.Password!);
        }

        public async Task<User> GetByUsername(string username)
        {
            return await _dbSet.FirstAsync(x => x.UserName == username);
        }
    }
}
