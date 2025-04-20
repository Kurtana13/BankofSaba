using BankofSaba.API.Authorization;
using Microsoft.AspNetCore.Authorization;

public static class AuthorizationService
{
    public static IServiceCollection AuthorizationServices(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationHandler, SelfOrAdminHandler>();
        services.AddHttpContextAccessor(); 

        services.AddAuthorization(options =>
        {
            options.AddPolicy("SelfOrAdmin", policy =>
                policy.Requirements.Add(new SelfOrAdminRequirement()));
        });

        return services;
    }
}