using ELearningPTIT.Modules.Courses.Application.Commands;
using ELearningPTIT.Modules.Courses.Application.DTOs;
using ELearningPTIT.Modules.Courses.Domain.Entities;
using ELearningPTIT.Modules.Courses.Domain.Exceptions;
using ELearningPTIT.Modules.Courses.Domain.Repositories;
using Mapster;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Courses.Application.Handlers;

public class CreateQuestionCommandHandler(
    IQuestionRepository questionRepository,
    ICourseRepository courseRepository
) : ICommandHandler<CreateQuestionCommand, QuestionDto>
{
    public async Task<QuestionDto> HandleAsync(CreateQuestionCommand command)
    {
        // Verify course exists
        var course = await courseRepository.GetAsync(command.CourseId);
        if (course == null)
        {
            throw new CourseNotFoundException(command.CourseId);
        }

        var question = new Question
        {
            CourseId = command.CourseId,
            LectureId = command.LectureId,
            UserId = command.UserId,
            Title = command.Title,
            Content = command.Content
        };

        await questionRepository.CreateAsync(question);

        return question.Adapt<QuestionDto>();
    }
}
