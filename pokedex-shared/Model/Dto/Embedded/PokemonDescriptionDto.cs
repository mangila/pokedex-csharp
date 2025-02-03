using System.Text.Json.Serialization;

namespace pokedex_shared.Model.Dto.Embedded;

public readonly record struct PokemonDescriptionDto(
    [property: JsonPropertyName("language")]
    string Language,
    [property: JsonPropertyName("description")]
    string Description
);