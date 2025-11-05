using ELearningPTIT.Modules.Users.Domain.Entities;

namespace ELearningPTIT.Modules.Users.Application.Abstractions;

/// <summary>
/// Service for generating and validating JWT tokens
/// </summary>
public interface IJwtTokenService
{
    /// <summary>
    /// Generate an access token for a user
    /// </summary>
    string GenerateAccessToken(User user);

    /// <summary>
    /// Generate a refresh token
    /// </summary>
    RefreshToken GenerateRefreshToken(string ipAddress);

    /// <summary>
    /// Validate an access token and return user ID
    /// </summary>
    string? ValidateAccessToken(string token);

    /// <summary>
    /// Get user ID from token without validation (for expired tokens)
    /// </summary>
    string? GetUserIdFromToken(string token);
}
