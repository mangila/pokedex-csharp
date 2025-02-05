using Microsoft.Extensions.Options;
using pokedex_shared.Common.Option;
using pokedex_shared.Integration.PokeApi;
using pokedex_shared.Integration.PokeApi.Response.EvolutionChain;
using pokedex_shared.Integration.PokeApi.Response.Generation;
using pokedex_shared.Integration.PokeApi.Response.Pokemon;
using pokedex_shared.Integration.PokeApi.Response.Species;
using pokedex_shared.Model.Domain;
using EvolutionChain = pokedex_shared.Integration.PokeApi.Response.Species.EvolutionChain;

namespace pokedex_poller;

public class PokemonHandler(
    IOptions<PokeApiOption> pokeApiOption,
    PokeApiClient pokeApiClient
)
{
    private readonly PokeApiOption _pokeApiOption = pokeApiOption.Value;

    public async Task<PokemonGenerationResponse> GetGenerationAsync(
        PokemonGeneration generation,
        CancellationToken cancellationToken = default)
    {
        return await pokeApiClient.GetAsync<PokemonGenerationResponse>(
            uri: GetPokemonGenerationRelativeUri(generation),
            cancellationToken: cancellationToken);
    }

    public async Task<PokemonSpeciesResponse> GetSpeciesAsync(
        PokemonSpecies species,
        CancellationToken cancellationToken = default
    )
    {
        return await pokeApiClient.GetAsync<PokemonSpeciesResponse>(
            uri: new Uri(species.Url),
            cancellationToken: cancellationToken);
    }

    public async Task<EvolutionChainResponse> GetEvolutionChainAsync(
        EvolutionChain evolutionChain,
        CancellationToken cancellationToken)
    {
        return await pokeApiClient.GetAsync<EvolutionChainResponse>(
            uri: new Uri(evolutionChain.Url),
            cancellationToken: cancellationToken);
    }

    public async Task<List<PokemonResponse>> GetVarietiesAsync(PokemonSpeciesResponse species,
        CancellationToken cancellationToken)
    {
        var l = new List<Task<PokemonResponse>>();
        foreach (var variety in species.Varieties)
        {
            l.Add(
                pokeApiClient.GetAsync<PokemonResponse>(
                    uri: new Uri(variety.Pokemon.Url),
                    cancellationToken: cancellationToken)
            );
        }

        var varieties = await Task.WhenAll(l);
        return varieties.ToList();
    }

    private Uri GetPokemonGenerationRelativeUri(PokemonGeneration generation)
    {
        return new Uri(
            $"{_pokeApiOption.GetPokemonGenerationUri}"
                .Replace("{id}", generation.Value)
            , UriKind.Relative);
    }
}