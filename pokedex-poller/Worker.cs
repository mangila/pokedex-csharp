using System.Diagnostics;
using Microsoft.Extensions.Options;
using pokedex_shared.Common.Option;
using pokedex_shared.Database.Command;
using pokedex_shared.Integration.PokeApi;
using pokedex_shared.Model.Document.Embedded;
using pokedex_shared.Model.Domain;

namespace pokedex_poller;

public class Worker(
    ILogger<Worker> logger,
    IOptions<WorkerOption> workerOption,
    PokemonGeneration pokemonGeneration,
    MongoDbCommandRepository mongoDbCommandRepository,
    PokemonHandler pokemonHandler,
    PokemonMediaHandler pokemonMediaHandler,
    Action<string, bool> onWorkerCompleted)
    : BackgroundService
{
    private readonly string _pokemonGeneration = pokemonGeneration.Value;
    private readonly WorkerOption _workerOption = workerOption.Value;
    private readonly Random _random = new();

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("worker started: {time} - {pokemonGeneration}",
            DateTimeOffset.Now,
            _pokemonGeneration);
        return base.StartAsync(cancellationToken);
    }

    private Task CompleteAsync(TimeSpan stopWatchElapsed)
    {
        string elapsedTime =
            $"{stopWatchElapsed.Hours:00}:{stopWatchElapsed.Minutes:00}:{stopWatchElapsed.Seconds:00}.{stopWatchElapsed.Milliseconds / 10:00}";
        logger.LogInformation("worker ran to completion: {time} - {pokemonGeneration} - it took {elapsedTime}",
            DateTimeOffset.Now,
            _pokemonGeneration,
            elapsedTime);
        onWorkerCompleted.Invoke(_pokemonGeneration, true);
        return Task.CompletedTask;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var counter = new Counter(1);
            var generation = await pokemonHandler.GetGenerationAsync(
                pokemonGeneration,
                cancellationToken);
            foreach (var generationPokemon in generation.PokemonSpecies)
            {
                // Wait
                await Task.Delay(TimeSpan.FromSeconds(GetJitter()), cancellationToken);
                logger.LogInformation("{name} - ({count}/{length}) - {generation}",
                    generationPokemon.Name,
                    counter.ToString(),
                    generation.PokemonSpecies.Length,
                    _pokemonGeneration);

                // Fetch
                var species = await pokemonHandler.GetSpeciesAsync(
                    generationPokemon,
                    cancellationToken);
                var evolutionChain =
                    await pokemonHandler.GetEvolutionChainAsync(
                        species.EvolutionChain,
                        cancellationToken);
                var pokemonVarieties = await pokemonHandler.GetVarietiesAsync(
                    species,
                    cancellationToken);
                var pokemonDocuments = new List<PokemonDocument>();
                var varietyCounter = new Counter(1);
                foreach (var pokemon in pokemonVarieties)
                {
                    logger.LogInformation("{name} - ({count}/{length}) - {variety}",
                        pokemon.Name,
                        varietyCounter.ToString(),
                        pokemonVarieties.Count,
                        $"{species.Name} - {pokemon.Name}");
                    var name = new PokemonName(pokemon.Name);
                    var images = await pokemonMediaHandler.FetchImagesAsync(
                        name: name,
                        sprites: pokemon.Sprites,
                        cancellationToken: cancellationToken);
                    var audios = await pokemonMediaHandler.FetchAudiosAsync(
                        name: name,
                        cries: pokemon.Cries,
                        cancellationToken: cancellationToken);
                    pokemonDocuments.Add(PokeApiMapper.ToPokemonDocument(
                        name: name,
                        isDefault: pokemon.Default,
                        weight: pokemon.Weight,
                        height: pokemon.Height,
                        types: pokemon.Types,
                        stats: pokemon.Stats,
                        images: images,
                        audios: audios
                    ));
                    varietyCounter.Increment();
                }

                var document = PokeApiMapper.ToSpeciesDocument(
                    id: new PokemonId(species.Id),
                    name: new PokemonName(species.Name),
                    generation: pokemonGeneration,
                    region: generation.Region.Name,
                    evolutionChain: evolutionChain,
                    species: species,
                    pokemons: pokemonDocuments);

                await mongoDbCommandRepository.ReplaceOneAsync(
                    document: document,
                    cancellationToken: cancellationToken
                );

                counter.Increment();
            }

            stopWatch.Stop();
            await CompleteAsync(stopWatch.Elapsed);
        }
        catch (OperationCanceledException)
        {
            // Do nothing, stopping...
        }
        catch (Exception e)
        {
            logger.LogError(e, "ERR: {Message}", e.Message);
            Environment.Exit(1);
        }
    }

    private int GetJitter()
    {
        return _random.Next(
            _workerOption.Interval.Min,
            _workerOption.Interval.Max);
    }

    private class Counter(int initialValue)
    {
        private int Value { get; set; } = initialValue;

        public void Increment()
        {
            Value++;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}