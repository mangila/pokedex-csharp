using pokedex_poller.Config;
using pokedex_poller.Http;

namespace pokedex_poller;

public class Worker(
    ILogger<Worker> logger,
    WorkerOption workerOption,
    IEnumerable<int> ids,
    PokemonClient pokemonClient
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                foreach (var index in ids)
                {
                    var s = await pokemonClient.GetPokemon(index.ToString(), cancellationToken);
                    logger.LogInformation(s.Name);
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