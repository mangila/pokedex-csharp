using System.Text.Json.Serialization;

namespace pokedex_shared.Model.Dto.Embedded;

public readonly record struct PokemonGeneraDto(
    [property: JsonPropertyName("language")] string Language,
    [property: JsonPropertyName("genera")] string Genera
);
