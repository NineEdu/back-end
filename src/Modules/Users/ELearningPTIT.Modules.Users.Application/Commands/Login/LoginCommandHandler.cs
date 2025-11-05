using ELearningPTIT.Modules.Users.Application.Abstractions;
using ELearningPTIT.Modules.Users.Application.DTOs;
using ELearningPTIT.Modules.Users.Domain.Exceptions;
using ELearningPTIT.Modules.Users.Domain.Repositories;
using MediatR;

namespace ELearningPTIT.Modules.Users.Application.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService
    )
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // Find user by email
        var user = await _userRepository.GetByEmailAsync(request.Email.ToLowerInvariant(), cancellationToken);
        if (user == null)
        {
            throw new InvalidCredentialsException();
        }

        // Verify password
        if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
            throw new InvalidCredentialsException();
        }

        // Check if user is active
        if (!user.IsActive)
        {
            throw new UserInactiveException();
        }

        // Generate tokens
        var accessToken = _jwtTokenService.GenerateAccessToken(user);
        var refreshToken = _jwtTokenService.GenerateRefreshToken(request.IpAddress);

        // Remove old inactive refresh tokens
        user.RefreshTokens.RemoveAll(rt => !rt.IsActive);

        // Add new refresh token
        user.RefreshTokens.Add(refreshToken);

        // Update user
        user.UpdatedAt = DateTime.UtcNow;
        await _userRepository.ReplaceAsync(user);

        return new AuthResponse(
            accessToken,
            refreshToken.Token,
            user.ToDto()
        );
    }
}
