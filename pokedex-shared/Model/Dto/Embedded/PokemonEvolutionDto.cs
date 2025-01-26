using System.Text.Json.Serialization;

namespace pokedex_shared.Model.Dto.Embedded;

public readonly record struct PokemonEvolutionDto(
    [property: JsonPropertyName("value")] int Value,
    [property: JsonPropertyName("name")] string Name);