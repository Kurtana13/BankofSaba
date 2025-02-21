using BankofSaba.API.Models;
using BankofSaba.API.Models.ViewModels;

namespace BankofSaba.API.Repositories.IRepositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> CreateAsync(RegisterViewModel registerViewModel);
        Task<User> GetByUsername(string username);
    }
}
