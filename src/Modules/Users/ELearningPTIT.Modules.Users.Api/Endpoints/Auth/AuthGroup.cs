using FastEndpoints;

namespace ELearningPTIT.Modules.Users.Api.Endpoints.Auth;

public sealed class AuthGroup : Group
{
    public AuthGroup()
    {
        Configure("auth", options =>
        {
            options.Tags("Auth");
        });
    }
}
