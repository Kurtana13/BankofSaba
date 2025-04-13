using BankofSaba.API.Infrastructure;
using BankofSaba.API.Models;
using BankofSaba.API.Repositories;
using BankofSaba.API.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;

namespace BankofSaba.API.Startup
{
    public static class DependencyInjectionService
    {
        public static IServiceCollection DependencyInjectionServices(this IServiceCollection services)
        {
            services.AddScoped<UserManager<User>>();
            services.AddScoped<RoleManager<IdentityRole>>();
            services.AddScoped<TokenProvider>();
            services.AddScoped<IUserRepository,UserRepository>();
            services.AddScoped<IAccountRepository,AccountRepository>();

            return services;
        }
    }
}
