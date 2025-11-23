using ELearningPTIT.Modules.Courses.Application.DTOs;
using FluentValidation;
using Wemogy.CQRS.Queries.Abstractions;

namespace ELearningPTIT.Modules.Courses.Application.Queries;

public class GetCoursesByInstructorQuery : IQuery<List<CourseDto>>
{
    public string InstructorId { get; set; } = string.Empty;
}

public class GetCoursesByInstructorQueryValidator : AbstractValidator<GetCoursesByInstructorQuery>
{
    public GetCoursesByInstructorQueryValidator()
    {
        RuleFor(x => x.InstructorId)
            .NotEmpty().WithMessage("Instructor ID is required");
    }
}
