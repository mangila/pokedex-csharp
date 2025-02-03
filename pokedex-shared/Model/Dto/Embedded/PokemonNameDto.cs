using System.Text.Json.Serialization;

namespace pokedex_shared.Model.Dto.Embedded;

public readonly record struct PokemonNameDto(
    [property: JsonPropertyName("language")]
    string Language,
    [property: JsonPropertyName("name")] string Name
);