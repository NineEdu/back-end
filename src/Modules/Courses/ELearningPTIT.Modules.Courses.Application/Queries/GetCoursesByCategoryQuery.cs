using ELearningPTIT.Modules.Courses.Application.DTOs;
using FluentValidation;
using Wemogy.CQRS.Queries.Abstractions;

namespace ELearningPTIT.Modules.Courses.Application.Queries;

public class GetCoursesByCategoryQuery : IQuery<List<CourseDto>>
{
    public string CategoryId { get; set; } = string.Empty;
}

public class GetCoursesByCategoryQueryValidator : AbstractValidator<GetCoursesByCategoryQuery>
{
    public GetCoursesByCategoryQueryValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Category ID is required");
    }
}
