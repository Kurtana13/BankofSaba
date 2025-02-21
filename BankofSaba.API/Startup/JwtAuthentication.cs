using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BankofSaba.API.Startup
{
    public static class JwtAuthenticationSetup
    {
        public static IServiceCollection JwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var secretKey = configuration["JwtKey:Secret"]!;
            var issuer = configuration["Jwt:Issuer"];
            var audience = configuration["Jwt:Audience"];

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });


            return services;
        }
    }
}
