using System.Text.Json.Serialization;

namespace pokedex_shared.Http.PokemonGeneration;

public readonly record struct PokemonGenerationApiResponse(
    [property: JsonPropertyName("pokemon_species")]
    PokemonSpecies[] pokemonSpecies
);

public readonly record struct PokemonSpecies(
    string name
);