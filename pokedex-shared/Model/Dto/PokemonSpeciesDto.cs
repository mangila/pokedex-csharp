using System.Collections.Immutable;
using System.Text.Json.Serialization;
using pokedex_shared.Model.Dto.Embedded;

namespace pokedex_shared.Model.Dto;

public readonly record struct PokemonSpeciesDto(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("names")] ImmutableList<PokemonNameDto> Names,
    [property: JsonPropertyName("descriptions")]
    ImmutableList<PokemonDescriptionDto> Descriptions,
    [property: JsonPropertyName("genera")] ImmutableList<PokemonGeneraDto> Genera,
    [property: JsonPropertyName("pedigree")]
    PokemonPedigreeDto Pedigree,
    [property: JsonPropertyName("evolutions")]
    ImmutableList<PokemonEvolutionDto> Evolutions,
    [property: JsonPropertyName("varieties")]
    ImmutableList<PokemonDto> Varieties,
    [property: JsonPropertyName("special")]
    PokemonSpecialDto Special
);