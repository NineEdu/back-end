namespace ELearningPTIT.Modules.Media.Api.Endpoints.Media.GetMediaUrl;

public class GetMediaUrlRequest
{
    public required string Id { get; set; }
    public int? ExpiresInMinutes { get; set; }
}
