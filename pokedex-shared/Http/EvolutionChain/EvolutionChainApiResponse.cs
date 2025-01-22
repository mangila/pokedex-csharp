using System.Text.Json.Serialization;

namespace pokedex_shared.Http.EvolutionChain;

public readonly record struct EvolutionChainApiResponse(Chain chain);

public readonly record struct Chain(
    [property: JsonPropertyName("evolves_to")]
    EvolutionChain[] chain,
    Species species
);

public readonly record struct EvolutionChain(
    [property: JsonPropertyName("evolves_to")]
    EvolutionChain[] next,
    Species species
);

public readonly record struct Species(string name);