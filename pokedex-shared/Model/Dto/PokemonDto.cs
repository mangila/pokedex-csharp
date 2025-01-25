using System.Text.Json.Serialization;

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
    [property: JsonPropertyName("images")] List<PokemonImageDto> Images,
    [property: JsonPropertyName("audios")] List<PokemonAudioDto> Audios,
    [property: JsonPropertyName("legendary")]
    bool Legendary,
    [property: JsonPropertyName("mythical")]
    bool Mythical,
    [property: JsonPropertyName("baby")] bool Baby);

public readonly record struct PokemonTypeDto(
    [property: JsonPropertyName("type")] string Type);

public readonly record struct PokemonEvolutionDto(
    [property: JsonPropertyName("value")] int Value,
    [property: JsonPropertyName("name")] string Name);

public readonly record struct PokemonStatDto(
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("value")] int Value);

public readonly record struct PokemonImageDto(
    [property: JsonPropertyName("media_id")]
    string MediaId,
    [property: JsonPropertyName("src")] string Src,
    [property: JsonPropertyName("file_name")]
    string FileName,
    [property: JsonPropertyName("content_type")]
    string ContentType);

public readonly record struct PokemonAudioDto(
    [property: JsonPropertyName("media_id")]
    string MediaId,
    [property: JsonPropertyName("src")] string Src,
    [property: JsonPropertyName("file_name")]
    string FileName,
    [property: JsonPropertyName("content_type")]
    string ContentType);