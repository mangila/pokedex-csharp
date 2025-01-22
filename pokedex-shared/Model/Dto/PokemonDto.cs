using System.Text.Json.Serialization;

namespace pokedex_shared.Model.Dto;

public readonly record struct PokemonDto(
    [property: JsonPropertyName("pokemon_id")]
    string PokemonId,
    [property: JsonPropertyName("name")] string Name
);