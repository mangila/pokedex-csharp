using System.Text.Json.Serialization;
using pokedex_shared.Model.Dto.Embedded;

namespace pokedex_shared.Model.Dto;

public readonly record struct PokemonDto(
    [property: JsonPropertyName("pokemon_id")]
    string PokemonId,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("region")] string Region,
    [property: JsonPropertyName("height")] string Height,
    [property: JsonPropertyName("weight")] string Weight,
    [property: JsonPropertyName("description")]
    string Description,
    [property: JsonPropertyName("generation")]
    string Generation,
    [property: JsonPropertyName("types")] List<PokemonTypeDto> Types,
    [property: JsonPropertyName("evolutions")]
    List<PokemonEvolutionDto> Evolutions,
    [property: JsonPropertyName("stats")] List<PokemonStatDto> Stats,
    [property: JsonPropertyName("images")] List<PokemonMediaDto> Images,
    [property: JsonPropertyName("audios")] List<PokemonMediaDto> Audios,
    [property: JsonPropertyName("legendary")]
    bool Legendary,
    [property: JsonPropertyName("mythical")]
    bool Mythical,
    [property: JsonPropertyName("baby")] bool Baby);