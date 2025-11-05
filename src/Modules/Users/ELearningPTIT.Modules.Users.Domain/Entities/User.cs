using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using YourCompany.YourProject.Shared.Core.Abstractions;
using YourCompany.YourProject.Shared.Core.Attributes;

namespace ELearningPTIT.Modules.Users.Domain.Entities;

/// <summary>
/// Represents a user in the system (Student, Instructor, or Admin)
/// </summary>
[CollectionName("users")]
public class User : EntityBase
{
    public User()
    {
        // Override EntityBase's Guid-based ID with MongoDB ObjectId
        Id = ObjectId.GenerateNewId().ToString();
    }

    [BsonElement("email")]
    [BsonRequired]
    public string Email { get; set; } = string.Empty;

    [BsonElement("passwordHash")]
    [BsonRequired]
    public string PasswordHash { get; set; } = string.Empty;

    [BsonElement("firstName")]
    public string FirstName { get; set; } = string.Empty;

    [BsonElement("lastName")]
    public string LastName { get; set; } = string.Empty;

    [BsonElement("phoneNumber")]
    public string? PhoneNumber { get; set; }

    [BsonElement("profileImageUrl")]
    public string? ProfileImageUrl { get; set; }

    [BsonElement("bio")]
    public string? Bio { get; set; }

    [BsonElement("roles")]
    public List<UserRole> Roles { get; set; } = new();

    [BsonElement("isEmailVerified")]
    public bool IsEmailVerified { get; set; } = false;

    [BsonElement("isActive")]
    public bool IsActive { get; set; } = true;

    [BsonElement("emailVerificationToken")]
    public string? EmailVerificationToken { get; set; }

    [BsonElement("passwordResetToken")]
    public string? PasswordResetToken { get; set; }

    [BsonElement("passwordResetTokenExpiry")]
    public DateTime? PasswordResetTokenExpiry { get; set; }

    [BsonElement("refreshTokens")]
    public List<RefreshToken> RefreshTokens { get; set; } = new();

    // For Instructors
    [BsonElement("instructorProfile")]
    public InstructorProfile? InstructorProfile { get; set; }

    // Helper methods
    public bool HasRole(UserRole role) => Roles.Contains(role);

    public bool IsInstructor() => HasRole(UserRole.Instructor);

    public bool IsAdmin() => HasRole(UserRole.Admin);

    public bool IsStudent() => HasRole(UserRole.Student);
}

/// <summary>
/// User roles in the system
/// </summary>
public enum UserRole
{
    Student = 1,
    Instructor = 2,
    Admin = 3
}

/// <summary>
/// Instructor-specific profile information
/// </summary>
public class InstructorProfile
{
    [BsonElement("headline")]
    public string? Headline { get; set; }

    [BsonElement("website")]
    public string? Website { get; set; }

    [BsonElement("socialLinks")]
    public SocialLinks? SocialLinks { get; set; }

    [BsonElement("payoutConfig")]
    public PayoutConfig? PayoutConfig { get; set; }

    [BsonElement("rating")]
    public double Rating { get; set; } = 0.0;

    [BsonElement("totalStudents")]
    public int TotalStudents { get; set; } = 0;

    [BsonElement("totalCourses")]
    public int TotalCourses { get; set; } = 0;

    [BsonElement("totalReviews")]
    public int TotalReviews { get; set; } = 0;
}

/// <summary>
/// Social media links for instructors
/// </summary>
public class SocialLinks
{
    [BsonElement("twitter")]
    public string? Twitter { get; set; }

    [BsonElement("linkedin")]
    public string? LinkedIn { get; set; }

    [BsonElement("youtube")]
    public string? YouTube { get; set; }

    [BsonElement("facebook")]
    public string? Facebook { get; set; }

    [BsonElement("github")]
    public string? Github { get; set; }
}

/// <summary>
/// Payout configuration for instructors (Stripe Connect)
/// </summary>
public class PayoutConfig
{
    [BsonElement("stripeAccountId")]
    public string? StripeAccountId { get; set; }

    [BsonElement("isPayoutEnabled")]
    public bool IsPayoutEnabled { get; set; } = false;

    [BsonElement("commissionRate")]
    public decimal CommissionRate { get; set; } = 0.37m; // 37% platform, 63% instructor

    [BsonElement("bankAccountLast4")]
    public string? BankAccountLast4 { get; set; }

    [BsonElement("currency")]
    public string Currency { get; set; } = "USD";
}

/// <summary>
/// Refresh token for JWT authentication
/// </summary>
public class RefreshToken
{
    [BsonElement("token")]
    public string Token { get; set; } = string.Empty;

    [BsonElement("expiresAt")]
    public DateTime ExpiresAt { get; set; }

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("revokedAt")]
    public DateTime? RevokedAt { get; set; }

    [BsonElement("replacedByToken")]
    public string? ReplacedByToken { get; set; }

    [BsonElement("createdByIp")]
    public string CreatedByIp { get; set; } = string.Empty;

    [BsonElement("revokedByIp")]
    public string? RevokedByIp { get; set; }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

    public bool IsRevoked => RevokedAt != null;

    public bool IsActive => !IsRevoked && !IsExpired;
}
