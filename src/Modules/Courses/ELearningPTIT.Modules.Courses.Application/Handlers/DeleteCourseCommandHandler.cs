using ELearningPTIT.Modules.Courses.Application.Commands;
using ELearningPTIT.Modules.Courses.Domain.Exceptions;
using ELearningPTIT.Modules.Courses.Domain.Repositories;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Courses.Application.Handlers;

public class DeleteCourseCommandHandler(
    ICourseRepository courseRepository,
    ICategoryRepository categoryRepository
) : ICommandHandler<DeleteCourseCommand>
{
    public async Task HandleAsync(DeleteCourseCommand command)
    {
        var course = await courseRepository.GetAsync(command.CourseId);
        if (course == null)
        {
            throw new CourseNotFoundException(command.CourseId);
        }

        await courseRepository.DeleteAsync(command.CourseId);
        await categoryRepository.DecrementCourseCountAsync(course.CategoryId);
    }
}
