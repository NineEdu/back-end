using ELearningPTIT.Modules.Users.Application.Abstractions;
using ELearningPTIT.Modules.Users.Application.DTOs;
using ELearningPTIT.Modules.Users.Domain.Exceptions;
using ELearningPTIT.Modules.Users.Domain.Repositories;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Users.Application.Commands.Login;

public class LoginCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IJwtTokenService jwtTokenService
) : ICommandHandler<LoginCommand, AuthResponse>
{
    public async Task<AuthResponse> HandleAsync(LoginCommand command)
    {
        var email = command.Email.ToLowerInvariant();

        var user = await userRepository.GetAsync(u => u.Email == email);
        if (user == null)
        {
            throw new InvalidCredentialsException();
        }

        if (!passwordHasher.VerifyPassword(command.Password, user.PasswordHash))
        {
            throw new InvalidCredentialsException();
        }

        if (!user.IsActive)
        {
            throw new UserInactiveException();
        }

        var accessToken = jwtTokenService.GenerateAccessToken(user);
        var refreshToken = jwtTokenService.GenerateRefreshToken(command.IpAddress);

        user.RefreshTokens.RemoveAll(rt => !rt.IsActive);
        user.RefreshTokens.Add(refreshToken);

        user.UpdatedAt = DateTime.UtcNow;
        await userRepository.ReplaceAsync(user);

        return new AuthResponse(
            accessToken,
            refreshToken.Token,
            user.ToDto()
        );
    }
}
