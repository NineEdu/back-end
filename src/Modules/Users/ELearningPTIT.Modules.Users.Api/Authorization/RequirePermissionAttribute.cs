using Microsoft.AspNetCore.Authorization;

namespace ELearningPTIT.Modules.Users.Api.Authorization;

/// <summary>
/// Specifies that the class or method that this attribute is applied to requires the specified permission.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class RequirePermissionAttribute : AuthorizeAttribute
{
    public RequirePermissionAttribute(string permission)
    {
        Policy = $"Permission:{permission}";
    }
}
