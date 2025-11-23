using ELearningPTIT.Modules.Courses.Application.DTOs;
using FluentValidation;
using Wemogy.CQRS.Queries.Abstractions;

namespace ELearningPTIT.Modules.Courses.Application.Queries;

public class GetPublishedCoursesQuery : IQuery<List<CourseDto>>
{
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 20;
}

public class GetPublishedCoursesQueryValidator : AbstractValidator<GetPublishedCoursesQuery>
{
    public GetPublishedCoursesQueryValidator()
    {
        RuleFor(x => x.Skip)
            .GreaterThanOrEqualTo(0).WithMessage("Skip must be greater than or equal to 0");

        RuleFor(x => x.Take)
            .InclusiveBetween(1, 100).WithMessage("Take must be between 1 and 100");
    }
}
