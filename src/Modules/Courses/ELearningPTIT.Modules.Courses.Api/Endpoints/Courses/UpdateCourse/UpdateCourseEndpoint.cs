using ELearningPTIT.Modules.Courses.Application.Commands;
using ELearningPTIT.Modules.Courses.Application.DTOs;
using FastEndpoints;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Courses.Api.Endpoints.Courses.UpdateCourse;

public class UpdateCourseEndpoint(
    Wemogy.CQRS.Commands.Abstractions.ICommandHandler<UpdateCourseCommand, CourseDto> commandHandler)
    : Endpoint<UpdateCourseRequest, CourseDto>
{
    public override void Configure()
    {
        Put("/{courseId}");
        Group<CoursesGroup>();
        // TODO: Add permission-based authorization: courses:update
    }

    public override async Task HandleAsync(UpdateCourseRequest req, CancellationToken ct)
    {
        var command = new UpdateCourseCommand
        {
            CourseId = req.CourseId,
            Title = req.Body.Title,
            Subtitle = req.Body.Subtitle,
            Description = req.Body.Description,
            CategoryId = req.Body.CategoryId,
            ThumbnailUrl = req.Body.ThumbnailUrl,
            PreviewVideoUrl = req.Body.PreviewVideoUrl,
            Price = req.Body.Price,
            DiscountPrice = req.Body.DiscountPrice,
            DifficultyLevel = req.Body.DifficultyLevel,
            Language = req.Body.Language,
            Subtitles = req.Body.Subtitles,
            LearningOutcomes = req.Body.LearningOutcomes,
            Requirements = req.Body.Requirements,
            TargetAudience = req.Body.TargetAudience
        };

        var result = await commandHandler.HandleAsync(command);
        await Send.ResponseAsync(result, 200, ct);
    }
}
