using FastEndpoints;

namespace ELearningPTIT.Modules.Media.Api.Endpoints.Media;

public sealed class MediaGroup : Group
{
    public MediaGroup()
    {
        Configure("media", options =>
        {
            options.Tags("Media");
        });
    }
}
