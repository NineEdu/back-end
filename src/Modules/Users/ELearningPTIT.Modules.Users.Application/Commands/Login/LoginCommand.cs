using ELearningPTIT.Modules.Users.Application.DTOs;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Users.Application.Commands.Login;

public class LoginCommand : ICommand<AuthResponse>
{
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string IpAddress { get; init; }
}
