using ELearningPTIT.Modules.Media.Application.DTOs;
using Wemogy.CQRS.Queries.Abstractions;

namespace ELearningPTIT.Modules.Media.Application.Queries;

public class GetMediaAssetsQuery : IQuery<IEnumerable<MediaAssetDto>>
{
    public required string UploaderId { get; init; }
}
