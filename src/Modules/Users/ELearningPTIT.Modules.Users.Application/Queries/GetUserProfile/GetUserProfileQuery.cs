using ELearningPTIT.Modules.Users.Application.DTOs;
using Wemogy.CQRS.Queries.Abstractions;

namespace ELearningPTIT.Modules.Users.Application.Queries.GetUserProfile;

public class GetUserProfileQuery : IQuery<UserDto>
{
    public required string UserId { get; init; }
}
