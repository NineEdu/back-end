using ELearningPTIT.Modules.Courses.Application.Commands;
using ELearningPTIT.Modules.Courses.Application.DTOs;
using FastEndpoints;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Courses.Api.Endpoints.Courses.UpdateLecture;

public class UpdateLectureEndpoint(ICommands commands)
    : Endpoint<UpdateLectureRequest, LectureDto>
{
    public override void Configure()
    {
        Put("/{CourseId}/sections/{SectionId}/lectures/{LectureId}");
        Group<CoursesGroup>();
    }

    public override async Task HandleAsync(UpdateLectureRequest req, CancellationToken ct)
    {
        var command = new UpdateLectureCommand
        {
            CourseId = req.CourseId,
            SectionId = req.SectionId,
            LectureId = req.LectureId,
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
        await Send.ResponseAsync(result, 200, ct);
    }
}
