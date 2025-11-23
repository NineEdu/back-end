using FluentValidation;

namespace ELearningPTIT.Modules.Media.Application.Commands.UploadMedia;

public class UploadMediaCommandValidator : AbstractValidator<UploadMediaCommand>
{
    private static readonly string[] AllowedContentTypes = new[]
    {
        "video/mp4", "video/webm", "video/quicktime", "video/x-msvideo",
        "audio/mpeg", "audio/wav", "audio/ogg", "audio/webm",
        "image/jpeg", "image/png", "image/gif", "image/webp", "image/svg+xml",
        "application/pdf",
        "application/msword",
        "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        "application/vnd.ms-powerpoint",
        "application/vnd.openxmlformats-officedocument.presentationml.presentation"
    };

    private const long MaxFileSize = 500 * 1024 * 1024; // 500 MB

    public UploadMediaCommandValidator()
    {
        RuleFor(x => x.FileName)
            .NotEmpty().WithMessage("File name is required")
            .MaximumLength(255).WithMessage("File name must not exceed 255 characters");

        RuleFor(x => x.ContentType)
            .NotEmpty().WithMessage("Content type is required")
            .Must(BeAllowedContentType).WithMessage("File type is not allowed");

        RuleFor(x => x.FileSize)
            .GreaterThan(0).WithMessage("File size must be greater than 0")
            .LessThanOrEqualTo(MaxFileSize).WithMessage($"File size must not exceed {MaxFileSize / 1024 / 1024} MB");

        RuleFor(x => x.UploadedBy)
            .NotEmpty().WithMessage("Uploader ID is required");

        RuleFor(x => x.FileStream)
            .NotNull().WithMessage("File stream is required");
    }

    private static bool BeAllowedContentType(string contentType)
    {
        return AllowedContentTypes.Contains(contentType.ToLowerInvariant());
    }
}
