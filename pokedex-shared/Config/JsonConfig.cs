using System.Text.Json;
using System.Text.Json.Serialization;

namespace pokedex_shared.Config;

public static class JsonConfig
{
    public static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower, // Use SnakeCaseLower for properties
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower) } // Serialize enums as strings
    };
}