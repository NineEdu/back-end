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
        // Find user by email
        var user = await userRepository.GetByEmailAsync(command.Email.ToLowerInvariant());
        if (user == null)
        {
            throw new InvalidCredentialsException();
        }

        // Verify password
        if (!passwordHasher.VerifyPassword(command.Password, user.PasswordHash))
        {
            throw new InvalidCredentialsException();
        }

        // Check if user is active
        if (!user.IsActive)
        {
            throw new UserInactiveException();
        }

        // Generate tokens
        var accessToken = jwtTokenService.GenerateAccessToken(user);
        var refreshToken = jwtTokenService.GenerateRefreshToken(command.IpAddress);

        // Remove old inactive refresh tokens
        user.RefreshTokens.RemoveAll(rt => !rt.IsActive);

        // Add new refresh token
        user.RefreshTokens.Add(refreshToken);

        // Update user
        user.UpdatedAt = DateTime.UtcNow;
        await userRepository.ReplaceAsync(user);

        return new AuthResponse(
            accessToken,
            refreshToken.Token,
            user.ToDto()
        );
    }
}
