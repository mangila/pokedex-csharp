using System.Text.Json.Serialization;

namespace pokedex_shared.Model.Dto.Embedded;

public readonly record struct PokemonTypeDto(
    [property: JsonPropertyName("type")] string Type);