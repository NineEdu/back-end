using ELearningPTIT.Modules.Users.Application.Abstractions;
using ELearningPTIT.Modules.Users.Application.DTOs;
using ELearningPTIT.Modules.Users.Domain.Entities;
using ELearningPTIT.Modules.Users.Domain.Exceptions;
using ELearningPTIT.Modules.Users.Domain.Repositories;
using MediatR;

namespace ELearningPTIT.Modules.Users.Application.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public RegisterCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService
    )
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // Check if user already exists
        var existingUser = await _userRepository.GetByEmailAsync(request.Email.ToLowerInvariant(), cancellationToken);
        if (existingUser != null)
        {
            throw new UserAlreadyExistsException(request.Email);
        }

        // Parse roles
        var roles = request.Roles
            .Select(r => Enum.Parse<UserRole>(r, ignoreCase: true))
            .ToList();

        // Create user
        var user = new User
        {
            Email = request.Email.ToLowerInvariant(),
            PasswordHash = _passwordHasher.HashPassword(request.Password),
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            Roles = roles,
            IsEmailVerified = false, // Will be verified via email
            IsActive = true,
            EmailVerificationToken = Guid.NewGuid().ToString(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // If user is an instructor, initialize instructor profile
        if (user.IsInstructor())
        {
            user.InstructorProfile = new InstructorProfile
            {
                Rating = 0.0,
                TotalStudents = 0,
                TotalCourses = 0,
                TotalReviews = 0
            };
        }

        // Generate tokens
        var accessToken = _jwtTokenService.GenerateAccessToken(user);
        var refreshToken = _jwtTokenService.GenerateRefreshToken(request.IpAddress);

        user.RefreshTokens.Add(refreshToken);

        // Save user
        await _userRepository.CreateAsync(user);

        // TODO: Send verification email (will be implemented in Notification Service)

        return new AuthResponse(
            accessToken,
            refreshToken.Token,
            user.ToDto()
        );
    }
}
