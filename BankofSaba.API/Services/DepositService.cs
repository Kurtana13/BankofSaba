
using Newtonsoft.Json;

namespace BankofSaba.API.Services
{
    public class DepositService : IDepositService
    {
        private readonly HttpClient _httpClient;
        private readonly string _basePath = "api/deposit/";

        public DepositService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<string>> GetAllAsync(string userId)
        {
            var uri = $"{_basePath}{nameof(GetAllAsync)}?userId={userId}";
            var accountIds = await _httpClient.GetFromJsonAsync<IEnumerable<string>>(uri);
            return accountIds ?? Enumerable.Empty<string>();    
        }

        public async Task CreateAsync(string name, string userId, string accountId)
        {
            var uri = $"{_basePath}Create" +
                      $"?name={Uri.EscapeDataString(name)}" +
                      $"&userId={Uri.EscapeDataString(userId)}" +
                      $"&accountId={Uri.EscapeDataString(accountId)}";

            var response = await _httpClient.PostAsync(uri, null);
            response.EnsureSuccessStatusCode();
        }
    }
}
