using System.Text.Json;
using System.Text.Json.Serialization;

namespace pokedex_shared.Config;

public static class JsonConfig
{
    public static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.KebabCaseLower, // Use camelCase for properties
        WriteIndented = true, // Pretty-print JSON
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.KebabCaseLower) } // Serialize enums as strings
    };
}