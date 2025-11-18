using ELearningPTIT.Modules.Courses.Application.DTOs;
using FluentValidation;
using Wemogy.CQRS.Queries.Abstractions;

namespace ELearningPTIT.Modules.Courses.Application.Queries;

public class GetCourseReviewsQuery : IQuery<List<ReviewDto>>
{
    public string CourseId { get; set; } = string.Empty;
}

public class GetCourseReviewsQueryValidator : AbstractValidator<GetCourseReviewsQuery>
{
    public GetCourseReviewsQueryValidator()
    {
        RuleFor(x => x.CourseId)
            .NotEmpty().WithMessage("Course ID is required");
    }
}
