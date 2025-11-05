using FastEndpoints;

namespace ELearningPTIT.Modules.Users.Api.Endpoints.Users;

public sealed class UsersGroup : Group
{
    public UsersGroup()
    {
        Configure("users", options =>
        {
            options.Tags("Users");
        });
    }
}
