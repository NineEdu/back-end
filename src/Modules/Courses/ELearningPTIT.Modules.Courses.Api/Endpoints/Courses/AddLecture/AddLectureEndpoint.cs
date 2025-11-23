using ELearningPTIT.Modules.Courses.Application.Commands;
using ELearningPTIT.Modules.Courses.Application.DTOs;
using FastEndpoints;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Courses.Api.Endpoints.Courses.AddLecture;

public class AddLectureEndpoint(ICommands commands)
    : Endpoint<AddLectureRequest, LectureDto>
{
    public override void Configure()
    {
        Post("/{CourseId}/sections/{SectionId}/lectures");
        Group<CoursesGroup>();
    }

    public override async Task HandleAsync(AddLectureRequest req, CancellationToken ct)
    {
        var command = new AddLectureCommand
        {
            CourseId = req.CourseId,
            SectionId = req.SectionId,
            Title = req.Title,
            Description = req.Description,
            Type = req.Type,
            Order = req.Order,
            DurationMinutes = req.DurationMinutes,
            VideoUrl = req.VideoUrl,
            ArticleContent = req.ArticleContent,
            ResourceUrls = req.ResourceUrls,
            IsPreviewable = req.IsPreviewable
        };

        var result = await commands.RunAsync(command);
        await Send.ResponseAsync(result, 201, ct);
    }
}
