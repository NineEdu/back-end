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
        var courseId = Route<string>("courseId")!;

        var command = new UpdateCourseCommand
        {
            CourseId = courseId,
            Title = req.Title,
            Subtitle = req.Subtitle,
            Description = req.Description,
            CategoryId = req.CategoryId,
            ThumbnailUrl = req.ThumbnailUrl,
            PreviewVideoUrl = req.PreviewVideoUrl,
            Price = req.Price,
            DiscountPrice = req.DiscountPrice,
            DifficultyLevel = req.DifficultyLevel,
            Language = req.Language,
            Subtitles = req.Subtitles,
            LearningOutcomes = req.LearningOutcomes,
            Requirements = req.Requirements,
            TargetAudience = req.TargetAudience
        };

        var result = await commandHandler.HandleAsync(command);
        await Send.ResponseAsync(result, 200, ct);
    }
}
