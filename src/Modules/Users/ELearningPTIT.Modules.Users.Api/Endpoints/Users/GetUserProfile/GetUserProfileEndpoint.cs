using ELearningPTIT.Modules.Users.Application.DTOs;
using ELearningPTIT.Modules.Users.Application.Queries.GetUserProfile;
using ELearningPTIT.Modules.Users.Domain.ValueObjects;
using FastEndpoints;
using Wemogy.CQRS.Queries.Abstractions;

namespace ELearningPTIT.Modules.Users.Api.Endpoints.Users.GetUserProfile;

public class GetUserProfileEndpoint(Wemogy.CQRS.Queries.Abstractions.IQueryHandler<GetUserProfileQuery, UserDto> queryHandler)
    : Endpoint<GetUserProfileRequest, UserDto>
{
    public override void Configure()
    {
        Get("/{UserId}");
        Group<UsersGroup>();
        Permissions($"Permission:{EndpointPermissions.UsersRead}");
    }

    public override async Task HandleAsync(GetUserProfileRequest req, CancellationToken ct)
    {
        var query = new GetUserProfileQuery { UserId = req.UserId };
        var result = await queryHandler.HandleAsync(query, ct);
        await Send.ResponseAsync(result, 200, ct);
    }
}
