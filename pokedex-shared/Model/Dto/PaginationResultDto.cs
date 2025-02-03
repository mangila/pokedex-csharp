using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace pokedex_shared.Model.Dto;

public record PaginationResultDto(
    [property: JsonPropertyName("total_count")]
    long TotalCount,
    [property: JsonPropertyName("total_pages")]
    int TotalPages,
    [property: JsonPropertyName("current_page")]
    int CurrentPage,
    [property: JsonPropertyName("page_size")]
    int PageSize,
    [property: JsonPropertyName("documents")]
    ImmutableList<PokemonSpeciesDto> Documents
);