using System.Text.Json.Serialization;

namespace pokedex_shared.Model.Dto.Embedded;

public readonly record struct PokemonPedigreeDto(
    [property: JsonPropertyName("generation")]
    string Generation,
    [property: JsonPropertyName("region")] string Region
);