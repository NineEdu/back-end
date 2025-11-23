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
        var email = command.Email.ToLowerInvariant();

        var existingUser = await userRepository.GetAsync(u => u.Email == email);
        if (existingUser != null)
        {
            throw new UserAlreadyExistsException(command.Email);
        }

        var roles = command.Roles
            .Select(r => Enum.Parse<UserRole>(r, ignoreCase: true))
            .ToList();

        var user = new User
        {
            Email = email,
            PasswordHash = passwordHasher.HashPassword(command.Password),
            FirstName = command.FirstName,
            LastName = command.LastName,
            PhoneNumber = command.PhoneNumber,
            Roles = roles,
            IsEmailVerified = false,
            IsActive = true,
            EmailVerificationToken = Guid.NewGuid().ToString()
        };

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

        var accessToken = jwtTokenService.GenerateAccessToken(user);
        var refreshToken = jwtTokenService.GenerateRefreshToken(command.IpAddress);

        user.RefreshTokens.Add(refreshToken);

        await userRepository.CreateAsync(user);

        return new AuthResponse(
            accessToken,
            refreshToken.Token,
            user.ToDto()
        );
    }
}
