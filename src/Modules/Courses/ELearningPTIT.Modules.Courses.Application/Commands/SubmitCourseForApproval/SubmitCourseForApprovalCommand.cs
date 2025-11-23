using ELearningPTIT.Modules.Courses.Application.DTOs;
using FluentValidation;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Courses.Application.Commands;

public class SubmitCourseForApprovalCommand : ICommand<CourseDto>
{
    public string CourseId { get; set; } = string.Empty;
}

public class SubmitCourseForApprovalCommandValidator : AbstractValidator<SubmitCourseForApprovalCommand>
{
    public SubmitCourseForApprovalCommandValidator()
    {
        RuleFor(x => x.CourseId)
            .NotEmpty().WithMessage("Course ID is required");
    }
}
