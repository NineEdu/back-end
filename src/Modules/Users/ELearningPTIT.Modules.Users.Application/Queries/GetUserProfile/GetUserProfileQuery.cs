using ELearningPTIT.Modules.Users.Application.DTOs;
using MediatR;

namespace ELearningPTIT.Modules.Users.Application.Queries.GetUserProfile;

public record GetUserProfileQuery(string UserId) : IRequest<UserDto>;
