using System.Text;
using ELearningPTIT.Modules.Users.Api.Authorization;
using ELearningPTIT.Modules.Users.Application;
using ELearningPTIT.Modules.Users.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ELearningPTIT.Modules.Users.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddUsersModule(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // Register application services (CQRS, FluentValidation)
        services.AddApplicationServices();

        // Register infrastructure services (repositories, services)
        services.AddInfrastructureServices(configuration);

        // Add HTTP Context Accessor for FastEndpoints
        services.AddHttpContextAccessor();

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
            .AddPolicy($"Permission:{Domain.ValueObjects.EndpointPermissions.UsersRead}",
                policy => policy.Requirements.Add(new PermissionRequirement(Domain.ValueObjects.EndpointPermissions.UsersRead)))
            .AddPolicy($"Permission:{Domain.ValueObjects.EndpointPermissions.UsersWrite}",
                policy => policy.Requirements.Add(new PermissionRequirement(Domain.ValueObjects.EndpointPermissions.UsersWrite)))
            .AddPolicy($"Permission:{Domain.ValueObjects.EndpointPermissions.UsersDelete}",
                policy => policy.Requirements.Add(new PermissionRequirement(Domain.ValueObjects.EndpointPermissions.UsersDelete)))
            .AddPolicy($"Permission:{Domain.ValueObjects.EndpointPermissions.CoursesRead}",
                policy => policy.Requirements.Add(new PermissionRequirement(Domain.ValueObjects.EndpointPermissions.CoursesRead)))
            .AddPolicy($"Permission:{Domain.ValueObjects.EndpointPermissions.CoursesWrite}",
                policy => policy.Requirements.Add(new PermissionRequirement(Domain.ValueObjects.EndpointPermissions.CoursesWrite)))
            .AddPolicy($"Permission:{Domain.ValueObjects.EndpointPermissions.AdminAccess}",
                policy => policy.Requirements.Add(new PermissionRequirement(Domain.ValueObjects.EndpointPermissions.AdminAccess)));

        return services;
    }
}
