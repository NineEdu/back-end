using ELearningPTIT.Modules.Courses.Application.Commands;
using ELearningPTIT.Modules.Courses.Application.DTOs;
using ELearningPTIT.Modules.Courses.Domain.Exceptions;
using ELearningPTIT.Modules.Courses.Domain.Repositories;
using Mapster;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Courses.Application.Handlers;

public class UpdateCourseCommandHandler(
    ICourseRepository courseRepository,
    ICategoryRepository categoryRepository
) : ICommandHandler<UpdateCourseCommand, CourseDto>
{
    public async Task<CourseDto> HandleAsync(UpdateCourseCommand command)
    {
        var course = await courseRepository.GetAsync(command.CourseId);
        if (course == null)
        {
            throw new CourseNotFoundException(command.CourseId);
        }

        // Verify category exists if changed
        if (course.CategoryId != command.CategoryId)
        {
            var category = await categoryRepository.GetAsync(command.CategoryId);
            if (category == null)
            {
                throw new CategoryNotFoundException(command.CategoryId);
            }

            // Update category counts
            await categoryRepository.DecrementCourseCountAsync(course.CategoryId);
            await categoryRepository.IncrementCourseCountAsync(command.CategoryId);
        }

        // Update course properties
        course.Title = command.Title;
        course.Subtitle = command.Subtitle;
        course.Description = command.Description;
        course.CategoryId = command.CategoryId;
        course.ThumbnailUrl = command.ThumbnailUrl;
        course.PreviewVideoUrl = command.PreviewVideoUrl;
        course.Price = command.Price;
        course.DiscountPrice = command.DiscountPrice;
        course.DifficultyLevel = command.DifficultyLevel;
        course.Language = command.Language;
        course.Subtitles = command.Subtitles;
        course.LearningOutcomes = command.LearningOutcomes;
        course.Requirements = command.Requirements;
        course.TargetAudience = command.TargetAudience;
        course.LastUpdatedAt = DateTime.UtcNow;

        await courseRepository.ReplaceAsync(course);

        return course.Adapt<CourseDto>();
    }
}
