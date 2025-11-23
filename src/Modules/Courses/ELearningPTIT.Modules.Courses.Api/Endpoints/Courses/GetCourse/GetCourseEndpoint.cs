using ELearningPTIT.Modules.Courses.Application.DTOs;
using ELearningPTIT.Modules.Courses.Application.Queries;
using FastEndpoints;
using Wemogy.CQRS.Queries.Abstractions;

namespace ELearningPTIT.Modules.Courses.Api.Endpoints.Courses.GetCourse;

public class GetCourseEndpoint(IQueries queries)
    : Endpoint<GetCourseRequest, CourseDto?>
{
    public override void Configure()
    {
        Get("/{courseId}");
        Group<CoursesGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetCourseRequest req, CancellationToken ct)
    {
        var query = new GetCourseByIdQuery { CourseId = req.CourseId };
        var result = await queries.QueryAsync(query, ct);

        if (result == null)
        {
            await Send.ResponseAsync(null, 404, ct);
            return;
        }

        await Send.ResponseAsync(result, 200, ct);
    }
}
