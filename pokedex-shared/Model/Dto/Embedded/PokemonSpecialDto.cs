using System.Text.Json.Serialization;

namespace pokedex_shared.Model.Dto.Embedded;

public readonly record struct PokemonSpecialDto(
    [property: JsonPropertyName("special")]
    bool Special,
    [property: JsonPropertyName("legendary")]
    bool Legendary,
    [property: JsonPropertyName("mythical")]
    bool Mythical,
    [property: JsonPropertyName("baby")] bool Baby);