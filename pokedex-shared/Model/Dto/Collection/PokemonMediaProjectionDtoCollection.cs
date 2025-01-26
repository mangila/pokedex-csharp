using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace pokedex_shared.Model.Dto.Collection;

public readonly record struct PokemonMediaProjectionDtoCollection(
    [property: JsonPropertyName("pokemons")]
    ImmutableList<PokemonMediaProjectionDto> Collection
);