using pokedex_shared.Http;
using pokedex_shared.Http.EvolutionChain;
using pokedex_shared.Http.Pokemon;
using pokedex_shared.Http.PokemonGeneration;
using pokedex_shared.Http.Species;
using pokedex_shared.Mapper;
using pokedex_shared.Model.Document;
using pokedex_shared.Model.Domain;
using pokedex_shared.Option;
using pokedex_shared.Service;
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
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            logger.LogInformation("worker started - {pokemonGeneration}", pokemonGeneration);
            try
            {
                var generation =
                    await pokemonHttpClient.GetAsync<PokemonGenerationApiResponse>(GetPokemonGenerationRelativeUri(),
                        cancellationToken);
                foreach (var generationPokemon in generation.pokemonSpecies)
                {
                    logger.LogInformation("{name}", generationPokemon.name);
                    var pokemonName = new PokemonName(generationPokemon.name);
                    // Fetch 
                    var pokemon = await pokemonHttpClient.GetAsync<PokemonApiResponse>(
                        uri: GetPokemonRelativeUri(pokemonName),
                        cancellationToken: cancellationToken);
                    var species = await pokemonHttpClient.GetAsync<PokemonSpeciesApiResponse>(
                        uri: new Uri(pokemon.species.url),
                        cancellationToken: cancellationToken);
                    var evolutionChain = await pokemonHttpClient.GetAsync<EvolutionChainApiResponse>(
                        uri: new Uri(species.evolution_chain.url),
                        cancellationToken: cancellationToken);
                    var medias = await FetchMediaAsync(
                        name: pokemonName,
                        sprites: pokemon.sprites,
                        cries: pokemon.cries,
                        cancellationToken: cancellationToken);
                    // Map
                    var document = ApiMapper.ToDocument(
                        pokemonApiResponse: pokemon,
                        pokemonSpeciesApiResponse: species,
                        evolutionChainApiResponse: evolutionChain,
                        medias: medias
                    );
                    await mongoDbCommandService.ReplaceOneAsync(document, cancellationToken);
                    await Task.Delay(TimeSpan.FromSeconds(workerOption.Interval), cancellationToken);
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "ERR: {Message}", e.Message);
                Environment.Exit(1);
            }

            Environment.Exit(0);
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

    private async Task<List<PokemonMediaDocument>> FetchMediaAsync(
        PokemonName name,
        Sprites sprites,
        Cries cries,
        CancellationToken cancellationToken = default)
    {
        var tasks = new List<Task<PokemonMediaDocument>>();
        var imageContentType = "image/png";
        var audioContentType = "audio/ogg";
        var description = "Media from PokeAPI";

        if (sprites.front_default is not null)
        {
            tasks.Add(mongoDbGridFsCommandService.InsertAsync(
                uri: new Uri(sprites.front_default),
                fileName: GetImageFileName(name, "front-default"),
                contentType: imageContentType,
                description: description,
                cancellationToken: cancellationToken));
        }

        if (sprites.other.official_artwork.front_default is not null)
        {
            tasks.Add(mongoDbGridFsCommandService.InsertAsync(
                uri: new Uri(sprites.other.official_artwork.front_default),
                fileName: GetImageFileName(name, "official-artwork-front-default"),
                contentType: imageContentType,
                description: description,
                cancellationToken: cancellationToken));
        }

        if (cries.legacy is not null)
        {
            tasks.Add(mongoDbGridFsCommandService.InsertAsync(
                uri: new Uri(cries.legacy),
                fileName: GetAudioFileName(name, "legacy"),
                contentType: audioContentType,
                description: description,
                cancellationToken: cancellationToken));
        }

        if (cries.latest is not null)
        {
            tasks.Add(mongoDbGridFsCommandService.InsertAsync(
                uri: new Uri(cries.latest),
                fileName: GetAudioFileName(name, "latest"),
                contentType: audioContentType,
                description: description,
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