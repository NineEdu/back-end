using ELearningPTIT.Modules.Users.Application.Commands.Register;
using ELearningPTIT.Modules.Users.Application.DTOs;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Users.Api.Endpoints.Auth.Register;

public class RegisterEndpoint(
    ICommands commands,
    IHttpContextAccessor httpContextAccessor)
    : Endpoint<RegisterRequest, AuthResponse>
{
    public override void Configure()
    {
        Post("/register");
        Group<AuthGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(RegisterRequest req, CancellationToken ct)
    {
        var ipAddress = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        var command = new RegisterCommand
        {
            Email = req.Email,
            Password = req.Password,
            FirstName = req.FirstName,
            LastName = req.LastName,
            PhoneNumber = req.PhoneNumber,
            Roles = req.Roles ?? new List<string> { "Student" },
            IpAddress = ipAddress
        };

        var result = await commands.RunAsync(command);
        await Send.ResponseAsync(result, 200, ct);
    }
}
