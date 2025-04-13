using System.Security.Claims;

namespace BankofSaba.API.Startup
{
    public static class AuthorizationService
    {
        public static IServiceCollection AuthorizationServices(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("SelfOrAdmin", policy =>
                policy.RequireAssertion(context =>
                {
                    var userId = context.Resource as string;
                    var currentUserId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    var isAdmin = context.User.IsInRole("Admin");
                    var isOwner = currentUserId == userId;

                    return isAdmin || isOwner;
                }));
            });
            return services;
        }
    }
}
