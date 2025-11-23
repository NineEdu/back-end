using ELearningPTIT.Modules.Courses.Application.Commands;
using ELearningPTIT.Modules.Courses.Application.DTOs;
using ELearningPTIT.Modules.Courses.Domain.Exceptions;
using ELearningPTIT.Modules.Courses.Domain.Repositories;
using Mapster;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Courses.Application.Handlers;

public class UpdateLectureCommandHandler(
    ICourseRepository courseRepository
) : ICommandHandler<UpdateLectureCommand, LectureDto>
{
    public async Task<LectureDto> HandleAsync(UpdateLectureCommand command)
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

        var lecture = section.Lectures.FirstOrDefault(l => l.Id == command.LectureId);
        if (lecture == null)
        {
            throw new LectureNotFoundException(command.LectureId);
        }

        lecture.Title = command.Title;
        lecture.Description = command.Description;
        lecture.Type = command.Type;
        lecture.Order = command.Order;
        lecture.DurationMinutes = command.DurationMinutes;
        lecture.VideoUrl = command.VideoUrl;
        lecture.ArticleContent = command.ArticleContent;
        lecture.ResourceUrls = command.ResourceUrls;
        lecture.IsPreviewable = command.IsPreviewable;

        course.CalculateTotals();
        course.LastUpdatedAt = DateTime.UtcNow;

        await courseRepository.ReplaceAsync(course);

        return lecture.Adapt<LectureDto>();
    }
}
