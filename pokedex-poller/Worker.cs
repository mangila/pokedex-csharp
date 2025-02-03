using Microsoft.Extensions.Options;
using pokedex_shared.Http;
using pokedex_shared.Http.EvolutionChain;
using pokedex_shared.Http.Pokemon;
using pokedex_shared.Http.PokemonGeneration;
using pokedex_shared.Http.Species;
using pokedex_shared.Mapper;
using pokedex_shared.Model.Domain;
using pokedex_shared.Option;
using pokedex_shared.Service.Command;

namespace pokedex_poller;

public class Worker(
    ILogger<Worker> logger,
    IOptions<WorkerOption> workerOption,
    IOptions<PokeApiOption> pokeApiOption,
    PokemonGeneration pokemonGeneration,
    PokemonHttpClient pokemonHttpClient,
    MongoDbCommandService mongoDbCommandService,
    MongoDbGridFsCommandService mongoDbGridFsCommandService,
    MediaHandler mediaHandler,
    Random random,
    Action<string, bool> onWorkerStarted,
    Action<string, bool> onWorkerCompleted)
    : BackgroundService
{
    private readonly string _pokemonGeneration = pokemonGeneration.Value;
    private readonly PokeApiOption _pokeApiOption = pokeApiOption.Value;
    private readonly WorkerOption _workerOption = workerOption.Value;
    private bool _isDone;

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("worker started: {time} - {pokemonGeneration}",
            DateTimeOffset.Now,
            _pokemonGeneration);
        onWorkerStarted.Invoke(_pokemonGeneration, _isDone);
        return base.StartAsync(cancellationToken);
    }

    private Task CompleteAsync()
    {
        logger.LogInformation("worker ran to completion: {time} - {pokemonGeneration}",
            DateTimeOffset.Now,
            _pokemonGeneration);
        _isDone = true;
        onWorkerCompleted.Invoke(_pokemonGeneration, _isDone);
        return Task.CompletedTask;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            var generation =
                await pokemonHttpClient.GetAsync<PokemonGenerationApiResponse>(
                    uri: GetPokemonGenerationRelativeUri(),
                    cancellationToken: cancellationToken);
            var count = 1;
            foreach (var generationPokemon in generation.PokemonSpecies)
            {
                // Wait
                await Task.Delay(TimeSpan.FromSeconds(GetJitter()), cancellationToken);
                logger.LogInformation("{name} - ({count}/{length}) - {generation}",
                    generationPokemon.Name,
                    count,
                    generation.PokemonSpecies.Length,
                    _pokemonGeneration);

                // Fetch
                var pokemonName = new PokemonName(generationPokemon.Name);
                var species =
                    await pokemonHttpClient.GetAsync<PokemonSpeciesApiResponse>(
                        uri: new Uri(generationPokemon.Url),
                        cancellationToken: cancellationToken);
                var evolutionChain = await pokemonHttpClient.GetAsync<EvolutionChainApiResponse>(
                    uri: new Uri(species.EvolutionChain.Url),
                    cancellationToken: cancellationToken);
                foreach (var variety in species.Varieties)
                {
                    var pokemon = await pokemonHttpClient.GetAsync<PokemonApiResponse>(
                        uri: new Uri(variety.Pokemon.Url),
                        cancellationToken: cancellationToken);
                    var pokemonId = new PokemonId(pokemon.Id);
                    var images = await mediaHandler.FetchImagesAsync(
                        name: pokemonName,
                        sprites: pokemon.Sprites,
                        cancellationToken: cancellationToken);
                    var audios = await mediaHandler.FetchAudiosAsync(
                        name: pokemonName,
                        cries: pokemon.Cries,
                        cancellationToken: cancellationToken);
                    var document = ApiMapper.ToDocument(
                        generation: _pokemonGeneration,
                        region: generation.Region.Name,
                        images: images,
                        audios: audios,
                        pokemonApiResponse: pokemon,
                        pokemonSpeciesApiResponse: species,
                        evolutionChainApiResponse: evolutionChain
                    );
                    await mongoDbCommandService.ReplaceOneAsync(
                        document: document,
                        cancellationToken: cancellationToken
                    );
                }

                // Map
                count++;
            }

            await CompleteAsync();
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
        return random.Next(
            _workerOption.Interval.Min,
            _workerOption.Interval.Max);
    }

    private Uri GetPokemonGenerationRelativeUri()
    {
        return new Uri(
            $"{_pokeApiOption.GetPokemonGenerationUri}"
                .Replace("{id}", _pokemonGeneration)
            , UriKind.Relative);
    }
}