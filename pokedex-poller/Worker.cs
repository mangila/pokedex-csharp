using pokedex_poller.Config;
using pokedex_shared.Http;
using pokedex_shared.Service;

namespace pokedex_poller;

public class Worker(
    ILogger<Worker> logger,
    WorkerOption workerOption,
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
                    var pokemon = await pokemonHttpClient.GetPokemon(index.ToString(), cancellationToken);
                    mongoDbService.InsertAsync(pokemon, cancellationToken);
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
}