using System.Text.Json.Serialization;

namespace pokedex_shared.Model;

public readonly record struct PokemonApiResponse(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("name")] string Name
);

public static partial class Extensions
{
    public static PokemonDocument ToDocument(this PokemonApiResponse apiResponse)
    {
        var document = new PokemonDocument
        {
            Id = apiResponse.Id,
            Name = apiResponse.Name
        };
        document.Validate();
        return document;
    }
}