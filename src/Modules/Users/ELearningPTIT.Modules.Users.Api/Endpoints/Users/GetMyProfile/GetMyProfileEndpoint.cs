using System.Security.Claims;
using ELearningPTIT.Modules.Users.Application.DTOs;
using ELearningPTIT.Modules.Users.Application.Queries.GetUserProfile;
using FastEndpoints;
using Wemogy.CQRS.Queries.Abstractions;

namespace ELearningPTIT.Modules.Users.Api.Endpoints.Users.GetMyProfile;

public class GetMyProfileEndpoint(IQueries queries)
    : EndpointWithoutRequest<UserDto>
{
    public override void Configure()
    {
        Get("/me");
        Group<UsersGroup>();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var query = new GetUserProfileQuery { UserId = userId };
        var result = await queries.QueryAsync(query, ct);
        await Send.ResponseAsync(result, 200, ct);
    }
}
