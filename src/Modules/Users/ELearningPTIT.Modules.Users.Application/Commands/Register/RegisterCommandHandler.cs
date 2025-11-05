using ELearningPTIT.Modules.Users.Application.Abstractions;
using ELearningPTIT.Modules.Users.Application.DTOs;
using ELearningPTIT.Modules.Users.Domain.Entities;
using ELearningPTIT.Modules.Users.Domain.Exceptions;
using ELearningPTIT.Modules.Users.Domain.Repositories;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Users.Application.Commands.Register;

public class RegisterCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IJwtTokenService jwtTokenService
) : ICommandHandler<RegisterCommand, AuthResponse>
{
    public async Task<AuthResponse> HandleAsync(RegisterCommand command)
    {
        // Check if user already exists
        var existingUser = await userRepository.GetByEmailAsync(command.Email.ToLowerInvariant());
        if (existingUser != null)
        {
            throw new UserAlreadyExistsException(command.Email);
        }

        // Parse roles
        var roles = command.Roles
            .Select(r => Enum.Parse<UserRole>(r, ignoreCase: true))
            .ToList();

        // Create user
        var user = new User
        {
            Email = command.Email.ToLowerInvariant(),
            PasswordHash = passwordHasher.HashPassword(command.Password),
            FirstName = command.FirstName,
            LastName = command.LastName,
            PhoneNumber = command.PhoneNumber,
            Roles = roles,
            IsEmailVerified = false, // Will be verified via email
            IsActive = true,
            EmailVerificationToken = Guid.NewGuid().ToString()
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
        var accessToken = jwtTokenService.GenerateAccessToken(user);
        var refreshToken = jwtTokenService.GenerateRefreshToken(command.IpAddress);

        user.RefreshTokens.Add(refreshToken);

        // Save user
        await userRepository.CreateAsync(user);

        // TODO: Send verification email (will be implemented in Notification Service)

        return new AuthResponse(
            accessToken,
            refreshToken.Token,
            user.ToDto()
        );
    }
}
