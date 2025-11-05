using ELearningPTIT.Modules.Users.Application.DTOs;
using MediatR;

namespace ELearningPTIT.Modules.Users.Application.Commands.Register;

public record RegisterCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string? PhoneNumber,
    List<string> Roles,
    string IpAddress
) : IRequest<AuthResponse>;
