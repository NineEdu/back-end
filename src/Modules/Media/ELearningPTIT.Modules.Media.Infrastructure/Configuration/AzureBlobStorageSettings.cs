namespace ELearningPTIT.Modules.Media.Infrastructure.Configuration;

public class AzureBlobStorageSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string ContainerName { get; set; } = "media";
}
