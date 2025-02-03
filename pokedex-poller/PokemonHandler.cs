using Microsoft.Extensions.Options;
using pokedex_shared.Common.Option;
using pokedex_shared.Integration.PokeApi;
using pokedex_shared.Integration.PokeApi.Response.Generation;
using pokedex_shared.Integration.PokeApi.Response.Pokemon;
using pokedex_shared.Integration.PokeApi.Response.Species;
using pokedex_shared.Model.Domain;

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

    public async Task<EvolutionChainApiResponse> GetEvolutionChainAsync(EvolutionChain evolutionChain,
        CancellationToken cancellationToken)
    {
        return await pokeApiClient.GetAsync<EvolutionChainApiResponse>(
            uri: new Uri(evolutionChain.Url),
            cancellationToken: cancellationToken);
    }

    public async Task<List<PokemonResponse>> GetVarietiesAsync(PokemonSpeciesResponse species,
        CancellationToken cancellationToken)
    {
        var l = new List<PokemonResponse>();
        foreach (var variety in species.Varieties)
        {
            l.Add(await pokeApiClient.GetAsync<PokemonResponse>(
                uri: new Uri(variety.Pokemon.Url),
                cancellationToken: cancellationToken));
        }

        return l;
    }


    private Uri GetPokemonGenerationRelativeUri(PokemonGeneration generation)
    {
        return new Uri(
            $"{_pokeApiOption.GetPokemonGenerationUri}"
                .Replace("{id}", generation.Value)
            , UriKind.Relative);
    }
}