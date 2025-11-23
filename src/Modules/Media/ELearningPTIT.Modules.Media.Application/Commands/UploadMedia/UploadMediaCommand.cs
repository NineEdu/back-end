using ELearningPTIT.Modules.Media.Application.DTOs;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Media.Application.Commands.UploadMedia;

public class UploadMediaCommand : ICommand<MediaAssetDto>
{
    public required Stream FileStream { get; init; }
    public required string FileName { get; init; }
    public required string ContentType { get; init; }
    public required long FileSize { get; init; }
    public required string UploadedBy { get; init; }
    public Dictionary<string, string>? Metadata { get; init; }
}
