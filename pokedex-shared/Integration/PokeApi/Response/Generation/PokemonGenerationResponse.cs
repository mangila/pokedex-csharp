using System.Text.Json.Serialization;

namespace pokedex_shared.Integration.PokeApi.Response.Generation;

public readonly record struct PokemonGenerationResponse(
    [property: JsonPropertyName("pokemon_species")]
    PokemonSpecies[] PokemonSpecies,
    [property: JsonPropertyName("main_region")]
    Region Region
);

public readonly record struct PokemonSpecies(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("url")] string Url
);

public readonly record struct Region(
    [property: JsonPropertyName("name")] string Name
);