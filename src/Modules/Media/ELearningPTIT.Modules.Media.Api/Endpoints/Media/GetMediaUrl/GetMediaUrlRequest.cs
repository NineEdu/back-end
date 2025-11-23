namespace ELearningPTIT.Modules.Media.Api.Endpoints.Media.GetMediaUrl;

public class GetMediaUrlRequest
{
    public string Id { get; set; } = string.Empty;
    public int? ExpiresInMinutes { get; set; }
}
