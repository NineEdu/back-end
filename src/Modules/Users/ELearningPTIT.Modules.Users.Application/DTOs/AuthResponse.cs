using ELearningPTIT.Modules.Users.Domain.Entities;

namespace ELearningPTIT.Modules.Users.Application.DTOs;

public record AuthResponse(
    string AccessToken,
    string RefreshToken,
    UserDto User
);

public record UserDto(
    string Id,
    string Email,
    string FirstName,
    string LastName,
    string? PhoneNumber,
    string? ProfileImageUrl,
    string? Bio,
    List<string> Roles,
    bool IsEmailVerified,
    InstructorProfileDto? InstructorProfile
);

public record InstructorProfileDto(
    string? Headline,
    string? Website,
    double Rating,
    int TotalStudents,
    int TotalCourses,
    int TotalReviews
);

public static class UserMapping
{
    public static UserDto ToDto(this User user)
    {
        return new UserDto(
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            user.PhoneNumber,
            user.ProfileImageUrl,
            user.Bio,
            user.Roles.Select(r => r.ToString()).ToList(),
            user.IsEmailVerified,
            user.InstructorProfile?.ToDto()
        );
    }

    public static InstructorProfileDto? ToDto(this InstructorProfile? profile)
    {
        if (profile == null) return null;

        return new InstructorProfileDto(
            profile.Headline,
            profile.Website,
            profile.Rating,
            profile.TotalStudents,
            profile.TotalCourses,
            profile.TotalReviews
        );
    }
}
