using ELearningPTIT.Modules.Courses.Application.DTOs;
using ELearningPTIT.Modules.Courses.Domain.ValueObjects;
using FluentValidation;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Courses.Application.Commands;

public class UpdateLectureCommand : ICommand<LectureDto>
{
    public string CourseId { get; set; } = string.Empty;
    public string SectionId { get; set; } = string.Empty;
    public string LectureId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public LectureType Type { get; set; } = LectureType.Video;
    public int Order { get; set; }
    public int DurationMinutes { get; set; }
    public string? VideoUrl { get; set; }
    public string? ArticleContent { get; set; }
    public List<string> ResourceUrls { get; set; } = new();
    public bool IsPreviewable { get; set; }
}

public class UpdateLectureCommandValidator : AbstractValidator<UpdateLectureCommand>
{
    public UpdateLectureCommandValidator()
    {
        RuleFor(x => x.CourseId)
            .NotEmpty().WithMessage("Course ID is required");

        RuleFor(x => x.SectionId)
            .NotEmpty().WithMessage("Section ID is required");

        RuleFor(x => x.LectureId)
            .NotEmpty().WithMessage("Lecture ID is required");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Lecture title is required")
            .MaximumLength(200).WithMessage("Lecture title must not exceed 200 characters");
    }
}
