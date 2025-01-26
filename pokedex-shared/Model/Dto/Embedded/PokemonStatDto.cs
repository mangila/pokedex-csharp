using System.Text.Json.Serialization;

namespace pokedex_shared.Model.Dto.Embedded;

public readonly record struct PokemonStatDto(
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("value")] int Value);