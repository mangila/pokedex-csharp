using pokedex_shared.Http;
using pokedex_shared.Http.EvolutionChain;
using pokedex_shared.Http.Pokemon;
using pokedex_shared.Http.PokemonGeneration;
using pokedex_shared.Http.Species;
using pokedex_shared.Mapper;
using pokedex_shared.Model.Domain;
using pokedex_shared.Option;
using pokedex_shared.Service;

namespace pokedex_poller;

public class Worker(
    ILogger<Worker> logger,
    WorkerOption workerOption,
    PokeApiOption pokeApiOption,
    PokemonGeneration pokemonGeneration,
    PokemonHttpClient pokemonHttpClient,
    MongoDbService mongoDbService,
    MongoDbGridFsService mongoDbGridFsService) : BackgroundService
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
                    var pokemon = await pokemonHttpClient.GetAsync<PokemonApiResponse>(
                        uri: GetPokemonRelativeUri(pokemonName),
                        cancellationToken: cancellationToken);
                    var species = await pokemonHttpClient.GetAsync<PokemonSpeciesApiResponse>(
                        uri: new Uri(pokemon.species.url),
                        cancellationToken: cancellationToken);
                    var evolutionChain = await pokemonHttpClient.GetAsync<EvolutionChainApiResponse>(
                        uri: new Uri(species.evolution_chain.url),
                        cancellationToken: cancellationToken);
                    var spriteId =
                        await mongoDbGridFsService.InsertAsync(
                            uri: new Uri(pokemon.sprites.front_default),
                            fileName: pokemonName.Value + "-sprite.png",
                            contentType: "image/png",
                            description: "Sprite from PokeAPI",
                            cancellationToken: cancellationToken);
                    var audioId = await mongoDbGridFsService.InsertAsync(
                        uri: new Uri(pokemon.cries.legacy ?? pokemon.cries.latest),
                        fileName: pokemonName.Value + "-audio.ogg",
                        contentType: "audio/ogg",
                        description: "Cry from PokeAPI",
                        cancellationToken: cancellationToken);
                    await mongoDbService.ReplaceOneAsync(
                        ApiMapper.ToDocument(
                            pokemonApiResponse: pokemon,
                            pokemonSpeciesApiResponse: species,
                            evolutionChainApiResponse: evolutionChain,
                            spriteId: spriteId,
                            audioId: audioId),
                        cancellationToken);
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
}