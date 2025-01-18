using System.Text.Json;
using System.Text.Json.Serialization;
using pokedex_shared.Config;

namespace pokedex_shared.Model;

public readonly record struct PokemonDto(
    [property: JsonPropertyName("pokemon_id")]
    string PokemonId,
    [property: JsonPropertyName("name")] string Name
);

public static partial class Extensions
{
    public static PokemonDocument ToDocument(this PokemonDto dto)
    {
        var document = new PokemonDocument
        {
            PokemonId = dto.PokemonId,
            Name = dto.Name
        };
        document.Validate();
        return document;
    }

    public static string ToJson(this PokemonDto dto)
    {
        return JsonSerializer.Serialize(dto, JsonConfig.JsonOptions);
    }

    public static async Task<string> ToJsonAsync(this PokemonDto dto, CancellationToken cancellationToken = default)
    {
        await using var memoryStream = new MemoryStream();
        await JsonSerializer.SerializeAsync(memoryStream, dto, JsonConfig.JsonOptions, cancellationToken);
        memoryStream.Seek(0, SeekOrigin.Begin); // Reset stream position to the beginning
        using var reader = new StreamReader(memoryStream);
        return await reader.ReadToEndAsync(cancellationToken);
    }
}