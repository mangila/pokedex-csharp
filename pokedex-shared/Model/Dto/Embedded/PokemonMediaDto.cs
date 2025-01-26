using System.Text.Json.Serialization;

namespace pokedex_shared.Model.Dto.Embedded;

public readonly record struct PokemonMediaDto(
    [property: JsonPropertyName("media_id")]
    string MediaId,
    [property: JsonPropertyName("src")] string Src,
    [property: JsonPropertyName("file_name")]
    string FileName,
    [property: JsonPropertyName("content_type")]
    string ContentType);