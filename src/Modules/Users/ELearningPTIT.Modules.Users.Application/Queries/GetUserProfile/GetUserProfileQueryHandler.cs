using ELearningPTIT.Modules.Users.Application.DTOs;
using ELearningPTIT.Modules.Users.Domain.Exceptions;
using ELearningPTIT.Modules.Users.Domain.Repositories;
using Wemogy.CQRS.Queries.Abstractions;

namespace ELearningPTIT.Modules.Users.Application.Queries.GetUserProfile;

public class GetUserProfileQueryHandler(IUserRepository userRepository)
    : IQueryHandler<GetUserProfileQuery, UserDto>
{
    public async Task<UserDto> HandleAsync(GetUserProfileQuery query, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetAsync(query.UserId, cancellationToken);

        if (user == null)
        {
            throw new UserNotFoundException(query.UserId);
        }

        return user.ToDto();
    }
}
