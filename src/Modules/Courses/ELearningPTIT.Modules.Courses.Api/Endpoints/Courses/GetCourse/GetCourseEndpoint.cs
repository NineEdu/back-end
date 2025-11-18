using ELearningPTIT.Modules.Courses.Application.DTOs;
using ELearningPTIT.Modules.Courses.Application.Queries;
using FastEndpoints;
using Wemogy.CQRS.Queries.Abstractions;

namespace ELearningPTIT.Modules.Courses.Api.Endpoints.Courses.GetCourse;

public class GetCourseEndpoint(
    IQueryHandler<GetCourseByIdQuery, CourseDto?> queryHandler)
    : EndpointWithoutRequest<CourseDto?>
{
    public override void Configure()
    {
        Get("/{courseId}");
        Group<CoursesGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var courseId = Route<string>("courseId")!;

        var query = new GetCourseByIdQuery { CourseId = courseId };
        var result = await queryHandler.HandleAsync(query, ct);

        if (result == null)
        {
            await Send.ResponseAsync(null, 404, ct);
            return;
        }

        await Send.ResponseAsync(result, 200, ct);
    }
}
