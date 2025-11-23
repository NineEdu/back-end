using ELearningPTIT.Modules.Courses.Application.Commands;
using ELearningPTIT.Modules.Courses.Application.DTOs;
using ELearningPTIT.Modules.Courses.Domain.Entities;
using ELearningPTIT.Modules.Courses.Domain.Exceptions;
using ELearningPTIT.Modules.Courses.Domain.Repositories;
using Mapster;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Courses.Application.Handlers;

public class AddLectureCommandHandler(
    ICourseRepository courseRepository
) : ICommandHandler<AddLectureCommand, LectureDto>
{
    public async Task<LectureDto> HandleAsync(AddLectureCommand command)
    {
        var course = await courseRepository.GetAsync(command.CourseId);
        if (course == null)
        {
            throw new CourseNotFoundException(command.CourseId);
        }

        var section = course.Sections.FirstOrDefault(s => s.Id == command.SectionId);
        if (section == null)
        {
            throw new SectionNotFoundException(command.SectionId);
        }

        var lecture = new Lecture
        {
            Title = command.Title,
            Description = command.Description,
            Type = command.Type,
            Order = command.Order > 0 ? command.Order : section.Lectures.Count + 1,
            DurationMinutes = command.DurationMinutes,
            VideoUrl = command.VideoUrl,
            ArticleContent = command.ArticleContent,
            ResourceUrls = command.ResourceUrls,
            IsPreviewable = command.IsPreviewable
        };

        section.Lectures.Add(lecture);
        course.CalculateTotals();
        course.LastUpdatedAt = DateTime.UtcNow;

        await courseRepository.ReplaceAsync(course);

        return lecture.Adapt<LectureDto>();
    }
}
