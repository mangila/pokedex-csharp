using System.Text.Json.Serialization;

namespace pokedex_shared.Integration.PokeApi.Response.Species;

public readonly record struct PokemonSpeciesResponse(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("names")] Names[] Names,
    [property: JsonPropertyName("genera")] Genera[] Genera,
    [property: JsonPropertyName("evolution_chain")]
    EvolutionChain EvolutionChain,
    [property: JsonPropertyName("flavor_text_entries")]
    FlavorTextEntries[] FlavorTextEntries,
    [property: JsonPropertyName("varieties")]
    Varieties[] Varieties,
    [property: JsonPropertyName("is_baby")]
    bool Baby,
    [property: JsonPropertyName("is_legendary")]
    bool Legendary,
    [property: JsonPropertyName("is_mythical")]
    bool Mythical
);

public readonly record struct Genera(
    [property: JsonPropertyName("language")]
    Language Language,
    [property: JsonPropertyName("genus")] string Genus
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