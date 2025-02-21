using BankofSaba.API.Infrastructure;
using BankofSaba.API.Models;
using BankofSaba.API.Repositories;
using BankofSaba.API.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;

namespace BankofSaba.API.Startup
{
    public static class DependencyInjectionServices
    {
        public static IServiceCollection DependencyInjectionService(this IServiceCollection services)
        {
            services.AddScoped<UserManager<User>>();
            services.AddScoped<RoleManager<IdentityRole>>();
            services.AddScoped<TokenProvider>();
            services.AddScoped<IUserRepository,UserRepository>();

            return services;
        }
    }
}
