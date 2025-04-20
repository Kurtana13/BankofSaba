using BankofSaba.API.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

public class SelfOrAdminHandler : AuthorizationHandler<SelfOrAdminRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SelfOrAdminHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SelfOrAdminRequirement requirement)
    {
        var httpContext = _httpContextAccessor.HttpContext;

        var routeUsername = httpContext?.Request.RouteValues["username"]?.ToString();
        var currentUsername = context.User.Identity?.Name;
        var isAdmin = context.User.IsInRole("ADMIN");

        if (routeUsername != null && (currentUsername == routeUsername || isAdmin))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
