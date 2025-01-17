using System.Text.Json.Serialization;

namespace pokedex_shared.Model;

public readonly record struct PokemonDto(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("name")] string Name
);

public static partial class Extensions
{
    public static PokemonDocument ToDocument(this PokemonDto dto)
    {
        var document = new PokemonDocument
        {
            Id = dto.Id,
            Name = dto.Name
        };
        document.Validate();
        return document;
    }
}