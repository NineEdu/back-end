using ELearningPTIT.Modules.Media.Application.DTOs;
using Wemogy.CQRS.Queries.Abstractions;

namespace ELearningPTIT.Modules.Media.Application.Queries;

public class GetMediaAssetQuery : IQuery<MediaAssetDto?>
{
    public required string MediaAssetId { get; init; }
}
