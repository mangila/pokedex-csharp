using pokedex_shared.Http;
using pokedex_shared.Http.EvolutionChain;
using pokedex_shared.Http.Pokemon;
using pokedex_shared.Http.Species;
using pokedex_shared.Mapper;
using pokedex_shared.Option;
using pokedex_shared.Service;

namespace pokedex_poller;

public class Worker(
    ILogger<Worker> logger,
    WorkerOption workerOption,
    PokeApiOption pokeApiOption,
    IEnumerable<int> ids,
    PokemonHttpClient pokemonHttpClient,
    MongoDbService mongoDbService) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                foreach (var index in ids)
                {
                    var pokemon = await pokemonHttpClient.Get<PokemonApiResponse>(
                        GetRelativeUri(index),
                        cancellationToken);
                    var species = await pokemonHttpClient.Get<SpeciesApiResponse>(new Uri(pokemon.species.url),
                        cancellationToken);
                    var evolutionChain = await pokemonHttpClient.Get<EvolutionChainApiResponse>(
                        new Uri(species.evolution_chain.url), cancellationToken);
                    await mongoDbService.InsertAsync(ApiMapper.ToDocument(pokemon, species, evolutionChain),
                        cancellationToken);
                    await Task.Delay(TimeSpan.FromSeconds(workerOption.Interval), cancellationToken);
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "ERR: {Message}", e.Message);
                Environment.Exit(1);
            }
        }
    }

    private Uri GetRelativeUri(int index)
    {
        return new Uri(
            $"{pokeApiOption.GetPokemonUri}".Replace("{id}", index.ToString())
            , UriKind.Relative);
    }
}