using FluentValidation;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Courses.Application.Commands;

public class DeleteLectureCommand : ICommand
{
    public string CourseId { get; set; } = string.Empty;
    public string SectionId { get; set; } = string.Empty;
    public string LectureId { get; set; } = string.Empty;
}

public class DeleteLectureCommandValidator : AbstractValidator<DeleteLectureCommand>
{
    public DeleteLectureCommandValidator()
    {
        RuleFor(x => x.CourseId)
            .NotEmpty().WithMessage("Course ID is required");

        RuleFor(x => x.SectionId)
            .NotEmpty().WithMessage("Section ID is required");

        RuleFor(x => x.LectureId)
            .NotEmpty().WithMessage("Lecture ID is required");
    }
}
