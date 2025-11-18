using ELearningPTIT.Modules.Courses.Application.Commands;
using ELearningPTIT.Modules.Courses.Application.DTOs;
using FastEndpoints;
using System.Security.Claims;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Courses.Api.Endpoints.Courses.CreateCourse;

public class CreateCourseEndpoint(
    Wemogy.CQRS.Commands.Abstractions.ICommandHandler<CreateCourseCommand, CourseDto> commandHandler)
    : Endpoint<CreateCourseRequest, CourseDto>
{
    public override void Configure()
    {
        Post("/");
        Group<CoursesGroup>();
        // TODO: Add permission-based authorization: courses:create
    }

    public override async Task HandleAsync(CreateCourseRequest req, CancellationToken ct)
    {
        // Get instructor ID from JWT claims
        var instructorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new UnauthorizedAccessException("User not authenticated");

        var command = new CreateCourseCommand
        {
            Title = req.Title,
            Subtitle = req.Subtitle,
            Description = req.Description,
            InstructorId = instructorId,
            CategoryId = req.CategoryId,
            Price = req.Price,
            DifficultyLevel = req.DifficultyLevel,
            Language = req.Language
        };

        var result = await commandHandler.HandleAsync(command);
        await Send.ResponseAsync(result, 201, ct);
    }
}
