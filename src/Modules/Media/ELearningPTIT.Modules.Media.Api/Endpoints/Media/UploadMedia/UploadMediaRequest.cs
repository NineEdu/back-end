using Microsoft.AspNetCore.Http;

namespace ELearningPTIT.Modules.Media.Api.Endpoints.Media.UploadMedia;

public class UploadMediaRequest
{
    public IFormFile File { get; set; } = null!;
}
