using ELearningPTIT.Modules.Courses.Application.DTOs;
using FluentValidation;
using Wemogy.CQRS.Queries.Abstractions;

namespace ELearningPTIT.Modules.Courses.Application.Queries;

public class GetCourseBySlugQuery : IQuery<CourseDto?>
{
    public string Slug { get; set; } = string.Empty;
}

public class GetCourseBySlugQueryValidator : AbstractValidator<GetCourseBySlugQuery>
{
    public GetCourseBySlugQueryValidator()
    {
        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Slug is required");
    }
}
