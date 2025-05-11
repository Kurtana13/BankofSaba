using BankofSaba.API.Infrastructure;
using BankofSaba.API.Models;
using BankofSaba.API.Repositories.IRepositories;
using BankofSaba.API.Repositories;
using Microsoft.AspNetCore.Identity;

namespace DepositService.API.Startup
{
    public static class DependencyInjectionService
    {
        public static IServiceCollection DependencyInjectionServices(this IServiceCollection services)
        {


            return services;
        }
    }
}
