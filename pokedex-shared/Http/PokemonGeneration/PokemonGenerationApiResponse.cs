using System.Text.Json.Serialization;

namespace pokedex_shared.Http.PokemonGeneration;

public readonly record struct PokemonGenerationApiResponse(
    [property: JsonPropertyName("pokemon_species")]
    PokemonSpecies[] PokemonSpecies,
    [property: JsonPropertyName("main_region")]
    Region Region
);

public readonly record struct PokemonSpecies(
    [property: JsonPropertyName("name")] string Name
);

public readonly record struct Region(
    [property: JsonPropertyName("name")] string Name
);