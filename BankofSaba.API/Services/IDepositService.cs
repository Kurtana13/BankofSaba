namespace BankofSaba.API.Services
{
    public interface IDepositService
    {
        Task<IEnumerable<string>> GetAllAsync(string userId);
        Task CreateAsync(string name, string userId, string accountId);
    }
}
