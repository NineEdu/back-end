using ELearningPTIT.Modules.Courses.Application.Commands;
using ELearningPTIT.Modules.Courses.Application.DTOs;
using ELearningPTIT.Modules.Courses.Domain.Entities;
using ELearningPTIT.Modules.Courses.Domain.Exceptions;
using ELearningPTIT.Modules.Courses.Domain.Repositories;
using ELearningPTIT.Modules.Courses.Domain.ValueObjects;
using Mapster;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Courses.Application.Handlers;

public class CreateCourseCommandHandler(
    ICourseRepository courseRepository,
    ICategoryRepository categoryRepository
) : ICommandHandler<CreateCourseCommand, CourseDto>
{
    public async Task<CourseDto> HandleAsync(CreateCourseCommand command)
    {
        // Verify category exists
        var category = await categoryRepository.GetAsync(command.CategoryId);
        if (category == null)
        {
            throw new CategoryNotFoundException(command.CategoryId);
        }

        // Generate slug from title
        var slug = GenerateSlug(command.Title);

        // Ensure slug is unique
        var existingCourse = await courseRepository.GetBySlugAsync(slug);
        if (existingCourse != null)
        {
            slug = $"{slug}-{Guid.NewGuid().ToString()[..8]}";
        }

        // Create course
        var course = new Course
        {
            Title = command.Title,
            Slug = slug,
            Subtitle = command.Subtitle,
            Description = command.Description,
            InstructorId = command.InstructorId,
            CategoryId = command.CategoryId,
            Price = command.Price,
            DifficultyLevel = command.DifficultyLevel,
            Language = command.Language,
            Status = CourseStatus.Draft,
            LastUpdatedAt = DateTime.UtcNow
        };

        await courseRepository.CreateAsync(course);
        await categoryRepository.IncrementCourseCountAsync(command.CategoryId);

        return course.Adapt<CourseDto>();
    }

    private static string GenerateSlug(string title)
    {
        return title.ToLowerInvariant()
            .Replace(" ", "-")
            .Replace("'", "")
            .Replace("\"", "")
            .Replace(".", "")
            .Replace(",", "")
            .Replace("!", "")
            .Replace("?", "");
    }
}
