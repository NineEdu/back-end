using ELearningPTIT.Modules.Courses.Application.DTOs;
using FluentValidation;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Courses.Application.Commands;

public class AddSectionCommand : ICommand<SectionDto>
{
    public string CourseId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Order { get; set; }
}

public class AddSectionCommandValidator : AbstractValidator<AddSectionCommand>
{
    public AddSectionCommandValidator()
    {
        RuleFor(x => x.CourseId)
            .NotEmpty().WithMessage("Course ID is required");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Section title is required")
            .MaximumLength(200).WithMessage("Section title must not exceed 200 characters");

        RuleFor(x => x.Order)
            .GreaterThanOrEqualTo(0).WithMessage("Order must be greater than or equal to 0");
    }
}
