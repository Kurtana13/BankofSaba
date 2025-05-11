using BankofSaba.API.Infrastructure;
using BankofSaba.API.Models;
using BankofSaba.API.Repositories;
using BankofSaba.API.Repositories.IRepositories;
using BankofSaba.API.Services;
using Microsoft.AspNetCore.Identity;

namespace BankofSaba.API.Startup
{
    public static class DependencyInjectionService
    {
        public static IServiceCollection DependencyInjectionServices(this IServiceCollection services,IConfiguration config)
        {
            services.AddScoped<UserManager<User>>();
            services.AddScoped<RoleManager<IdentityRole>>();
            services.AddScoped<TokenProvider>();
            services.AddScoped<IUserRepository,UserRepository>();
            services.AddScoped<IAccountRepository,AccountRepository>();
            services.AddScoped<IAccountService, AccountService>();

            services.AddMvc().AddControllersAsServices();

            services.AddHttpClient<IDepositService,DepositService>(client =>
            {
                client.BaseAddress = new Uri(config["DepositApiUrl"]);
            });
            return services;
        }
    }
}
