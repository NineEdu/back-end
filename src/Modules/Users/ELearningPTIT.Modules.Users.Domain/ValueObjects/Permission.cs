namespace ELearningPTIT.Modules.Users.Domain.ValueObjects;

/// <summary>
/// Defines granular permissions for Role-Based Access Control (RBAC)
/// </summary>
public static class EndpointPermissions
{
    // User Management
    public const string UsersRead = "users:read";
    public const string UsersWrite = "users:write";
    public const string UsersDelete = "users:delete";

    // Course Management
    public const string CoursesRead = "courses:read";
    public const string CoursesWrite = "courses:write";
    public const string CoursesDelete = "courses:delete";
    public const string CoursesPublish = "courses:publish";
    public const string CoursesApprove = "courses:approve";

    // Content Management
    public const string ContentRead = "content:read";
    public const string ContentWrite = "content:write";
    public const string ContentDelete = "content:delete";

    // Enrollment
    public const string EnrollmentsRead = "enrollments:read";
    public const string EnrollmentsWrite = "enrollments:write";
    public const string EnrollmentsDelete = "enrollments:delete";

    // Reviews
    public const string ReviewsRead = "reviews:read";
    public const string ReviewsWrite = "reviews:write";
    public const string ReviewsDelete = "reviews:delete";
    public const string ReviewsModerate = "reviews:moderate";

    // Orders & Payments
    public const string OrdersRead = "orders:read";
    public const string OrdersWrite = "orders:write";
    public const string OrdersRefund = "orders:refund";

    // Analytics
    public const string AnalyticsRead = "analytics:read";
    public const string AnalyticsExport = "analytics:export";

    // Payouts
    public const string PayoutsRead = "payouts:read";
    public const string PayoutsProcess = "payouts:process";

    // Admin
    public const string AdminAccess = "admin:access";
    public const string AdminSettings = "admin:settings";

    /// <summary>
    /// Get all permissions for a given role
    /// </summary>
    public static List<string> GetPermissionsForRole(string role)
    {
        return role.ToLower() switch
        {
            "student" => new List<string>
            {
                CoursesRead,
                ContentRead,
                EnrollmentsRead,
                EnrollmentsWrite,
                ReviewsRead,
                ReviewsWrite,
                OrdersRead,
                UsersRead // Own profile only
            },
            "instructor" => new List<string>
            {
                CoursesRead,
                CoursesWrite,
                CoursesDelete,
                CoursesPublish,
                ContentRead,
                ContentWrite,
                ContentDelete,
                EnrollmentsRead,
                ReviewsRead,
                AnalyticsRead,
                AnalyticsExport,
                PayoutsRead,
                UsersRead,
                UsersWrite // Own profile
            },
            "admin" => new List<string>
            {
                UsersRead,
                UsersWrite,
                UsersDelete,
                CoursesRead,
                CoursesWrite,
                CoursesDelete,
                CoursesPublish,
                CoursesApprove,
                ContentRead,
                ContentWrite,
                ContentDelete,
                EnrollmentsRead,
                EnrollmentsWrite,
                EnrollmentsDelete,
                ReviewsRead,
                ReviewsWrite,
                ReviewsDelete,
                ReviewsModerate,
                OrdersRead,
                OrdersWrite,
                OrdersRefund,
                AnalyticsRead,
                AnalyticsExport,
                PayoutsRead,
                PayoutsProcess,
                AdminAccess,
                AdminSettings
            },
            _ => new List<string>()
        };
    }
}
