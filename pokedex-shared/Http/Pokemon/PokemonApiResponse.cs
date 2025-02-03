using System.Text.Json.Serialization;

namespace pokedex_shared.Http.Pokemon;

public readonly record struct PokemonApiResponse(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("height")] int Height,
    [property: JsonPropertyName("weight")] int Weight,
    [property: JsonPropertyName("is_default")]
    bool Default,
    [property: JsonPropertyName("cries")] Cries Cries,
    [property: JsonPropertyName("sprites")]
    Sprites Sprites,
    [property: JsonPropertyName("stats")] Stats[] Stats,
    [property: JsonPropertyName("types")] Types[] Types,
    [property: JsonPropertyName("abilities")]
    Abilities[] Abilities
);

public readonly record struct Cries(
    [property: JsonPropertyName("latest")] string? Latest,
    [property: JsonPropertyName("legacy")] string? Legacy
);

public readonly record struct Stats(
    [property: JsonPropertyName("base_stat")]
    int BaseStat,
    [property: JsonPropertyName("stat")] Stat Stat
);

public readonly record struct Stat(
    [property: JsonPropertyName("name")] string Name
);

public readonly record struct Types(
    [property: JsonPropertyName("type")] Type Type
);

public readonly record struct Type(
    [property: JsonPropertyName("name")] string Name
);

public readonly record struct Abilities(
    [property: JsonPropertyName("ability")]
    Ability Ability
);

public readonly record struct Ability(
    [property: JsonPropertyName("url")] string Url
);