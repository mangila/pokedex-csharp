using System.Text.Json.Serialization;

namespace pokedex_shared.Model.Dto;

public readonly record struct PokemonDto(
    [property: JsonPropertyName("pokemon_id")]
    string PokemonId,
    [property: JsonPropertyName("name")] string Name,
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
    [property: JsonPropertyName("audio_id")]
    string AudioId,
    [property: JsonPropertyName("sprite_id")]
    string SpriteId,
    [property: JsonPropertyName("legendary")]
    bool Legendary,
    [property: JsonPropertyName("mythical")]
    bool Mythical,
    [property: JsonPropertyName("baby")] bool Baby
);

public readonly record struct PokemonTypeDto(string type);

public readonly record struct PokemonEvolutionDto(int value, string name);

public readonly record struct PokemonStatDto(string type, int value);