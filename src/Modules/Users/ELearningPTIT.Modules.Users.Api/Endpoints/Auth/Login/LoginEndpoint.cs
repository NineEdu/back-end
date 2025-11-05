using ELearningPTIT.Modules.Users.Application.Commands.Login;
using ELearningPTIT.Modules.Users.Application.DTOs;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Users.Api.Endpoints.Auth.Login;

public class LoginEndpoint(
    Wemogy.CQRS.Commands.Abstractions.ICommandHandler<LoginCommand, AuthResponse> commandHandler,
    IHttpContextAccessor httpContextAccessor)
    : Endpoint<LoginRequest, AuthResponse>
{
    public override void Configure()
    {
        Post("/login");
        Group<AuthGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        var ipAddress = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        var command = new LoginCommand
        {
            Email = req.Email,
            Password = req.Password,
            IpAddress = ipAddress
        };

        var result = await commandHandler.HandleAsync(command);
        await Send.ResponseAsync(result, 200, ct);
    }
}
