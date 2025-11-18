using ELearningPTIT.Modules.Courses.Application.DTOs;
using ELearningPTIT.Modules.Courses.Application.Queries;
using FastEndpoints;
using Wemogy.CQRS.Queries.Abstractions;

namespace ELearningPTIT.Modules.Courses.Api.Endpoints.Courses.GetPublishedCourses;

public class GetPublishedCoursesEndpoint(
    IQueryHandler<GetPublishedCoursesQuery, List<CourseDto>> queryHandler)
    : EndpointWithoutRequest<List<CourseDto>>
{
    public override void Configure()
    {
        Get("/published");
        Group<CoursesGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var skip = Query<int>("skip", isRequired: false);
        var take = Query<int>("take", isRequired: false);

        var query = new GetPublishedCoursesQuery
        {
            Skip = skip == 0 ? 0 : skip,
            Take = take == 0 ? 20 : take
        };

        var result = await queryHandler.HandleAsync(query, ct);
        await Send.ResponseAsync(result, 200, ct);
    }
}
