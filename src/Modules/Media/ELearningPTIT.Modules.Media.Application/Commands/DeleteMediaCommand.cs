using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Media.Application.Commands;

public class DeleteMediaCommand : ICommand
{
    public required string MediaAssetId { get; init; }
    public required string RequestedBy { get; init; }
}
