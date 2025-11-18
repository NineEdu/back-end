using ELearningPTIT.Modules.Courses.Application.Commands;
using ELearningPTIT.Modules.Courses.Application.DTOs;
using ELearningPTIT.Modules.Courses.Domain.Entities;
using ELearningPTIT.Modules.Courses.Domain.Repositories;
using Mapster;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Courses.Application.Handlers;

public class AddAnswerCommandHandler(
    IQuestionRepository questionRepository
) : ICommandHandler<AddAnswerCommand, QuestionDto>
{
    public async Task<QuestionDto> HandleAsync(AddAnswerCommand command)
    {
        var question = await questionRepository.GetAsync(command.QuestionId);
        if (question == null)
        {
            throw new Exception($"Question with ID '{command.QuestionId}' not found.");
        }

        var answer = new Answer
        {
            UserId = command.UserId,
            Content = command.Content,
            IsInstructorAnswer = command.IsInstructorAnswer
        };

        question.Answers.Add(answer);
        await questionRepository.ReplaceAsync(question);

        return question.Adapt<QuestionDto>();
    }
}
