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

public class Worker(
    ILogger<Worker> logger,
    WorkerOption workerOption,
    PokeApiOption pokeApiOption,
    PokemonGeneration pokemonGeneration,
    PokemonHttpClient pokemonHttpClient,
    MongoDbCommandService mongoDbCommandService,
    MongoDbGridFsCommandService mongoDbGridFsCommandService) : BackgroundService
{
    private const string ImageContentType = "image/png";
    private const string AudioContentType = "audio/ogg";
    private const string Description = "Media from PokeAPI";

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("worker started: {time} - {pokemonGeneration}",
            DateTime.Now,
            pokemonGeneration.Value);
        return base.StartAsync(cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("worker stopped: {time} - {pokemonGeneration}",
            DateTimeOffset.Now,
            pokemonGeneration.Value);
        return base.StopAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var generation =
                    await pokemonHttpClient.GetAsync<PokemonGenerationApiResponse>(GetPokemonGenerationRelativeUri(),
                        cancellationToken);
                var count = 1;
                foreach (var generationPokemon in generation.PokemonSpecies)
                {
                    logger.LogInformation("{name} - ({count}/{length}) - {generation}",
                        generationPokemon.Name,
                        count,
                        generation.PokemonSpecies.Length,
                        pokemonGeneration.Value);

                    var pokemonName = new PokemonName(generationPokemon.Name);
                    // Fetch 
                    var pokemon = await pokemonHttpClient.GetAsync<PokemonApiResponse>(
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
                    var species = await pokemonHttpClient.GetAsync<PokemonSpeciesApiResponse>(
                        uri: new Uri(pokemon.species.url),
                        cancellationToken: cancellationToken);
                    var evolutionChain = await pokemonHttpClient.GetAsync<EvolutionChainApiResponse>(
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
                    await mongoDbCommandService.ReplaceOneAsync(document, cancellationToken);
                    await Task.Delay(TimeSpan.FromSeconds(workerOption.Interval), cancellationToken);
                    count++;
                }
            }
            catch (OperationCanceledException)
            {
                // Do nothing
            }
            catch (Exception e)
            {
                logger.LogError(e, "ERR: {Message}", e.Message);
                Environment.Exit(1);
            }
        }
    }

    private Uri GetPokemonGenerationRelativeUri()
    {
        return new Uri(
            $"{pokeApiOption.GetPokemonGenerationUri}"
                .Replace("{id}", pokemonGeneration.Value)
            , UriKind.Relative);
    }

    private Uri GetPokemonRelativeUri(PokemonName name)
    {
        return new Uri(
            $"{pokeApiOption.GetPokemonUri}".Replace("{id}", name.Value)
            , UriKind.Relative);
    }

    private async Task<List<PokemonMediaDocument>> FetchCriesAsync(PokemonName name, Cries cries,
        CancellationToken cancellationToken)
    {
        var tasks = new List<Task<PokemonMediaDocument>>();

        if (cries.legacy is not null)
        {
            tasks.Add(mongoDbGridFsCommandService.InsertAsync(
                uri: new Uri(cries.legacy),
                fileName: GetAudioFileName(name, "legacy"),
                contentType: AudioContentType,
                description: Description,
                cancellationToken: cancellationToken));
        }

        if (cries.latest is not null)
        {
            tasks.Add(mongoDbGridFsCommandService.InsertAsync(
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
            tasks.Add(mongoDbGridFsCommandService.InsertAsync(
                uri: new Uri(sprites.front_default),
                fileName: GetImageFileName(name, "front-default"),
                contentType: ImageContentType,
                description: Description,
                cancellationToken: cancellationToken));
        }

        if (sprites.other.official_artwork.front_default is not null)
        {
            tasks.Add(mongoDbGridFsCommandService.InsertAsync(
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