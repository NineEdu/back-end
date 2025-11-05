using System.Text;
using ELearningPTIT.Modules.Users.Api.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ELearningPTIT.Modules.Users.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddUsersApi(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // Add JWT Authentication
        var jwtSettings = configuration.GetSection("Jwt");
        var secretKey = jwtSettings["Secret"] ?? throw new InvalidOperationException("JWT secret is not configured");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ValidateIssuer = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidateAudience = true,
                ValidAudience = jwtSettings["Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });

        // Add Authorization with permission-based policies
        services.AddAuthorization();
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

        // Register permission policies dynamically
        services.AddAuthorizationBuilder()
            .AddPolicy($"Permission:{Domain.ValueObjects.Permissions.UsersRead}",
                policy => policy.Requirements.Add(new PermissionRequirement(Domain.ValueObjects.Permissions.UsersRead)))
            .AddPolicy($"Permission:{Domain.ValueObjects.Permissions.UsersWrite}",
                policy => policy.Requirements.Add(new PermissionRequirement(Domain.ValueObjects.Permissions.UsersWrite)))
            .AddPolicy($"Permission:{Domain.ValueObjects.Permissions.UsersDelete}",
                policy => policy.Requirements.Add(new PermissionRequirement(Domain.ValueObjects.Permissions.UsersDelete)))
            .AddPolicy($"Permission:{Domain.ValueObjects.Permissions.CoursesRead}",
                policy => policy.Requirements.Add(new PermissionRequirement(Domain.ValueObjects.Permissions.CoursesRead)))
            .AddPolicy($"Permission:{Domain.ValueObjects.Permissions.CoursesWrite}",
                policy => policy.Requirements.Add(new PermissionRequirement(Domain.ValueObjects.Permissions.CoursesWrite)))
            .AddPolicy($"Permission:{Domain.ValueObjects.Permissions.AdminAccess}",
                policy => policy.Requirements.Add(new PermissionRequirement(Domain.ValueObjects.Permissions.AdminAccess)));

        return services;
    }
}
