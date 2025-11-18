using ELearningPTIT.Modules.Courses.Application.DTOs;
using FluentValidation;
using Wemogy.CQRS.Queries.Abstractions;

namespace ELearningPTIT.Modules.Courses.Application.Queries;

public class GetCourseQuestionsQuery : IQuery<List<QuestionDto>>
{
    public string CourseId { get; set; } = string.Empty;
}

public class GetCourseQuestionsQueryValidator : AbstractValidator<GetCourseQuestionsQuery>
{
    public GetCourseQuestionsQueryValidator()
    {
        RuleFor(x => x.CourseId)
            .NotEmpty().WithMessage("Course ID is required");
    }
}
