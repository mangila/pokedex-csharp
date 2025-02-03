using System.Text.Json.Serialization;

namespace pokedex_shared.Http.Species;

public readonly record struct PokemonSpeciesApiResponse(
    [property: JsonPropertyName("names")] Names[] Names,
    [property: JsonPropertyName("is_baby")]
    bool Baby,
    [property: JsonPropertyName("is_legendary")]
    bool Legendary,
    [property: JsonPropertyName("is_mythical")]
    bool Mythical,
    [property: JsonPropertyName("evolution_chain")]
    EvolutionChain EvolutionChain,
    [property: JsonPropertyName("flavor_text_entries")]
    FlavorTextEntries[] FlavorTextEntries,
    [property: JsonPropertyName("varieties")]
    Varieties[] Varieties
);

public readonly record struct Names(
    [property: JsonPropertyName("language")]
    Language Language,
    [property: JsonPropertyName("name")] string Name
);

public readonly record struct EvolutionChain(
    [property: JsonPropertyName("url")] string Url
);

public readonly record struct FlavorTextEntries(
    [property: JsonPropertyName("flavor_text")]
    string FlavorText,
    [property: JsonPropertyName("language")]
    Language Language
);

public readonly record struct Language(
    [property: JsonPropertyName("name")] string Name
);

public readonly record struct Varieties(
    [property: JsonPropertyName("is_default")]
    bool IsDefault,
    [property: JsonPropertyName("pokemon")]
    Pokemon Pokemon
);

public readonly record struct Pokemon(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("url")] string Url
);