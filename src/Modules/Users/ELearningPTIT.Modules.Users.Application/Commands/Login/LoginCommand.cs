using ELearningPTIT.Modules.Users.Application.DTOs;
using MediatR;

namespace ELearningPTIT.Modules.Users.Application.Commands.Login;

public record LoginCommand(
    string Email,
    string Password,
    string IpAddress
) : IRequest<AuthResponse>;
