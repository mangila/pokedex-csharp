using Microsoft.Extensions.Options;
using pokedex_shared.Http;
using pokedex_shared.Http.EvolutionChain;
using pokedex_shared.Http.Pokemon;
using pokedex_shared.Http.PokemonGeneration;
using pokedex_shared.Http.Species;
using pokedex_shared.Mapper;
using pokedex_shared.Model.Document.Embedded;
using pokedex_shared.Model.Domain;
using pokedex_shared.Option;
using pokedex_shared.Service.Command;

namespace pokedex_poller;

public class Worker : BackgroundService
{
    private const string ImageContentType = "image/png";
    private const string AudioContentType = "audio/ogg";
    private const string Description = "Media from PokeAPI";

    private readonly ILogger<Worker> _logger;
    private readonly string _pokemonGeneration;
    private readonly PokemonHttpClient _pokemonHttpClient;
    private readonly MongoDbCommandService _mongoDbCommandService;
    private readonly MongoDbGridFsCommandService _mongoDbGridFsCommandService;
    private readonly Random _random;
    private readonly Action<string, bool> _onWorkerStarted;
    private readonly Action<string, bool> _onWorkerCompleted;
    private readonly PokeApiOption _pokeApiOption;
    private readonly WorkerOption _workerOption;
    private bool _isDone;

    public Worker(
        ILogger<Worker> logger,
        IOptions<WorkerOption> workerOption,
        IOptions<PokeApiOption> pokeApiOption,
        PokemonGeneration pokemonGeneration,
        PokemonHttpClient pokemonHttpClient,
        MongoDbCommandService mongoDbCommandService,
        MongoDbGridFsCommandService mongoDbGridFsCommandService,
        Random random,
        Action<string, bool> onWorkerStarted,
        Action<string, bool> onWorkerCompleted)
    {
        _logger = logger;
        _pokemonGeneration = pokemonGeneration.Value;
        _pokemonHttpClient = pokemonHttpClient;
        _mongoDbCommandService = mongoDbCommandService;
        _mongoDbGridFsCommandService = mongoDbGridFsCommandService;
        _random = random;
        _onWorkerStarted = onWorkerStarted;
        _onWorkerCompleted = onWorkerCompleted;
        _pokeApiOption = pokeApiOption.Value;
        _workerOption = workerOption.Value;
        _isDone = false;
    }


    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("worker started: {time} - {pokemonGeneration}",
            DateTime.Now,
            _pokemonGeneration);
        _onWorkerStarted.Invoke(_pokemonGeneration, _isDone);
        return base.StartAsync(cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("worker stopped: {time} - {pokemonGeneration}",
            DateTimeOffset.Now,
            _pokemonGeneration);
        return base.StopAsync(cancellationToken);
    }

    private Task CompleteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("worker ran to completion: {time} - {pokemonGeneration}",
            DateTimeOffset.Now,
            _pokemonGeneration);
        _isDone = true;
        _onWorkerCompleted.Invoke(_pokemonGeneration, _isDone);
        return base.StopAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            var generation =
                await _pokemonHttpClient.GetAsync<PokemonGenerationApiResponse>(GetPokemonGenerationRelativeUri(),
                    cancellationToken);
            var count = 1;
            foreach (var generationPokemon in generation.PokemonSpecies)
            {
                _logger.LogInformation("{name} - ({count}/{length}) - {generation}",
                    generationPokemon.Name,
                    count,
                    generation.PokemonSpecies.Length,
                    _pokemonGeneration);

                var pokemonName = new PokemonName(generationPokemon.Name);
                // Fetch 
                var pokemon = await _pokemonHttpClient.GetAsync<PokemonApiResponse>(
                    uri: GetPokemonRelativeUri(pokemonName),
                    cancellationToken: cancellationToken);
                var images = await FetchImagesAsync(
                    name: pokemonName,
                    sprites: pokemon.sprites,
                    cancellationToken: cancellationToken
                );
                var cries = await FetchCriesAsync(
                    name: pokemonName,
                    cries: pokemon.cries,
                    cancellationToken: cancellationToken
                );
                var species = await _pokemonHttpClient.GetAsync<PokemonSpeciesApiResponse>(
                    uri: new Uri(pokemon.species.url),
                    cancellationToken: cancellationToken);
                var evolutionChain = await _pokemonHttpClient.GetAsync<EvolutionChainApiResponse>(
                    uri: new Uri(species.evolution_chain.url),
                    cancellationToken: cancellationToken);
                // Map
                var document = ApiMapper.ToDocument(
                    region: generation.Region.Name,
                    pokemonApiResponse: pokemon,
                    pokemonSpeciesApiResponse: species,
                    evolutionChainApiResponse: evolutionChain,
                    mediaCollection: [..images, ..cries]
                );
                await _mongoDbCommandService.ReplaceOneAsync(document, cancellationToken);
                var jitter = _random.Next(
                    _workerOption.Interval.Min,
                    _workerOption.Interval.Max);
                await Task.Delay(TimeSpan.FromSeconds(jitter), cancellationToken);
                count++;
            }

            await CompleteAsync(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            // Do nothing, stopping...
        }
        catch (Exception e)
        {
            _logger.LogError(e, "ERR: {Message}", e.Message);
            Environment.Exit(1);
        }
    }

    private Uri GetPokemonGenerationRelativeUri()
    {
        return new Uri(
            $"{_pokeApiOption.GetPokemonGenerationUri}"
                .Replace("{id}", _pokemonGeneration)
            , UriKind.Relative);
    }

    private Uri GetPokemonRelativeUri(PokemonName name)
    {
        return new Uri(
            $"{_pokeApiOption.GetPokemonUri}".Replace("{id}", name.Value)
            , UriKind.Relative);
    }

    private async Task<List<PokemonMediaDocument>> FetchCriesAsync(PokemonName name, Cries cries,
        CancellationToken cancellationToken)
    {
        var tasks = new List<Task<PokemonMediaDocument>>();

        if (cries.legacy is not null)
        {
            tasks.Add(_mongoDbGridFsCommandService.InsertAsync(
                uri: new Uri(cries.legacy),
                fileName: GetAudioFileName(name, "legacy"),
                contentType: AudioContentType,
                description: Description,
                cancellationToken: cancellationToken));
        }

        if (cries.latest is not null)
        {
            tasks.Add(_mongoDbGridFsCommandService.InsertAsync(
                uri: new Uri(cries.latest),
                fileName: GetAudioFileName(name, "latest"),
                contentType: AudioContentType,
                description: Description,
                cancellationToken: cancellationToken));
        }

        return (await Task.WhenAll(tasks)).ToList();
    }

    private async Task<List<PokemonMediaDocument>> FetchImagesAsync(PokemonName name, Sprites sprites,
        CancellationToken cancellationToken)
    {
        var tasks = new List<Task<PokemonMediaDocument>>();

        if (sprites.front_default is not null)
        {
            tasks.Add(_mongoDbGridFsCommandService.InsertAsync(
                uri: new Uri(sprites.front_default),
                fileName: GetImageFileName(name, "front-default"),
                contentType: ImageContentType,
                description: Description,
                cancellationToken: cancellationToken));
        }

        if (sprites.other.official_artwork.front_default is not null)
        {
            tasks.Add(_mongoDbGridFsCommandService.InsertAsync(
                uri: new Uri(sprites.other.official_artwork.front_default),
                fileName: GetImageFileName(name, "official-artwork-front-default"),
                contentType: ImageContentType,
                description: Description,
                cancellationToken: cancellationToken));
        }

        return (await Task.WhenAll(tasks)).ToList();
    }

    private static string GetImageFileName(PokemonName name, string type)
    {
        return $"{name.Value}-{type}.png";
    }

    private static string GetAudioFileName(PokemonName name, string type)
    {
        return $"{name.Value}-{type}.ogg";
    }
}