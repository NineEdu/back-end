using ELearningPTIT.Modules.Courses.Application.DTOs;
using FluentValidation;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Courses.Application.Commands;

public class ApproveCourseCommand : ICommand<CourseDto>
{
    public string CourseId { get; set; } = string.Empty;
    public string ApprovedBy { get; set; } = string.Empty;
}

public class ApproveCourseCommandValidator : AbstractValidator<ApproveCourseCommand>
{
    public ApproveCourseCommandValidator()
    {
        RuleFor(x => x.CourseId)
            .NotEmpty().WithMessage("Course ID is required");

        RuleFor(x => x.ApprovedBy)
            .NotEmpty().WithMessage("Approver ID is required");
    }
}
