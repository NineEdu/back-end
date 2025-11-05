using System.Text.Json;
using System.Text.Json.Serialization;

namespace YourCompany.YourProject.Shared.Core.Options;

public static class DefaultJsonSerializerOptions
{
    public static JsonSerializerOptions Create()
    {
        var jsonSerializerOptions = new JsonSerializerOptions();
        return jsonSerializerOptions.ApplyDefaultOptions();
    }

    public static JsonSerializerOptions ApplyDefaultOptions(this JsonSerializerOptions options)
    {
        options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.Converters.Add(
            new JsonStringEnumConverter(namingPolicy: JsonNamingPolicy.SnakeCaseUpper)
        );
        return options;
    }
}
