using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace pokedex_shared.Model.Dto.Embedded;

public readonly record struct PokemonDto(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("default")]
    bool Default,
    [property: JsonPropertyName("height")] string Height,
    [property: JsonPropertyName("weight")] string Weight,
    [property: JsonPropertyName("types")] ImmutableList<PokemonTypeDto> Types,
    [property: JsonPropertyName("stats")] ImmutableList<PokemonStatDto> Stats,
    [property: JsonPropertyName("images")] ImmutableList<PokemonMediaDto> Images,
    [property: JsonPropertyName("audios")] ImmutableList<PokemonMediaDto> Audios
);