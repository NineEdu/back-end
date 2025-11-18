using ELearningPTIT.Modules.Courses.Application.DTOs;
using ELearningPTIT.Modules.Courses.Application.Queries;
using ELearningPTIT.Modules.Courses.Domain.Repositories;
using Mapster;
using Wemogy.CQRS.Queries.Abstractions;

namespace ELearningPTIT.Modules.Courses.Application.Handlers;

public class GetCourseQuestionsQueryHandler(
    IQuestionRepository questionRepository
) : IQueryHandler<GetCourseQuestionsQuery, List<QuestionDto>>
{
    public async Task<List<QuestionDto>> HandleAsync(GetCourseQuestionsQuery query, CancellationToken cancellationToken)
    {
        var questions = await questionRepository.GetByCourseIdAsync(query.CourseId, cancellationToken);
        return questions.Adapt<List<QuestionDto>>();
    }
}
