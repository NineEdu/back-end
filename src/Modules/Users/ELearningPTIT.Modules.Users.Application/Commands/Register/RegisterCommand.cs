using ELearningPTIT.Modules.Users.Application.DTOs;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Users.Application.Commands.Register;

public class RegisterCommand : ICommand<AuthResponse>
{
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public string? PhoneNumber { get; init; }
    public required List<string> Roles { get; init; }
    public required string IpAddress { get; init; }
}
