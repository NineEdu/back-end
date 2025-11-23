using ELearningPTIT.Modules.Courses.Application.DTOs;
using FluentValidation;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Courses.Application.Commands;

public class UpdateSectionCommand : ICommand<SectionDto>
{
    public string CourseId { get; set; } = string.Empty;
    public string SectionId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Order { get; set; }
}

public class UpdateSectionCommandValidator : AbstractValidator<UpdateSectionCommand>
{
    public UpdateSectionCommandValidator()
    {
        RuleFor(x => x.CourseId)
            .NotEmpty().WithMessage("Course ID is required");

        RuleFor(x => x.SectionId)
            .NotEmpty().WithMessage("Section ID is required");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Section title is required")
            .MaximumLength(200).WithMessage("Section title must not exceed 200 characters");
    }
}
