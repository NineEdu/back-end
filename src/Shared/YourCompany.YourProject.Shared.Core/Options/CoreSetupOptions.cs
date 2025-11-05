namespace YourCompany.YourProject.Shared.Core.Options;

public class CoreSetupOptions
{
    public required DatabaseSetupOptions DatabaseSetupOptions { get; set; } = new();
}
