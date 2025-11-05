using ELearningPTIT.Modules.Users.Domain.Entities;
using YourCompany.YourProject.Shared.Core.Abstractions;

namespace ELearningPTIT.Modules.Users.Domain.Repositories;

/// <summary>
/// Repository interface for User entity with domain-specific queries
/// </summary>
public interface IUserRepository : IRepository<User>
{
    /// <summary>
    /// Find a user by email address
    /// </summary>
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Check if a user with the given email exists
    /// </summary>
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all users with a specific role
    /// </summary>
    Task<List<User>> GetByRoleAsync(UserRole role, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a user by refresh token
    /// </summary>
    Task<User?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a user by email verification token
    /// </summary>
    Task<User?> GetByEmailVerificationTokenAsync(string token, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a user by password reset token
    /// </summary>
    Task<User?> GetByPasswordResetTokenAsync(string token, CancellationToken cancellationToken = default);

    /// <summary>
    /// Search users by name or email (for admin)
    /// </summary>
    Task<List<User>> SearchUsersAsync(
        string searchTerm,
        int skip = 0,
        int take = 20,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Get instructors with pagination
    /// </summary>
    Task<(List<User> Instructors, long TotalCount)> GetInstructorsAsync(
        int skip = 0,
        int take = 20,
        CancellationToken cancellationToken = default
    );
}
