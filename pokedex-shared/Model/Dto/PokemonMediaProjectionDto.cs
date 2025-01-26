using System.Collections.Immutable;
using System.Text.Json.Serialization;
using pokedex_shared.Model.Dto.Embedded;

namespace pokedex_shared.Model.Dto;

public readonly record struct PokemonMediaProjectionDto(
    [property: JsonPropertyName("pokemon_id")]
    string PokemonId,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("images")] ImmutableList<PokemonMediaDto> Images);