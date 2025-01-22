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
                    var pokemonName = new PokemonName(generationPokemon.name);
                    logger.LogInformation("{name}", pokemonName.Value);
                    var pokemon = await pokemonHttpClient.GetAsync<PokemonApiResponse>(
                        GetPokemonRelativeUri(pokemonName),
                        cancellationToken);
                    var species = await pokemonHttpClient.GetAsync<PokemonSpeciesApiResponse>(
                        new Uri(pokemon.species.url),
                        cancellationToken);
                    var evolutionChain = await pokemonHttpClient.GetAsync<EvolutionChainApiResponse>(
                        new Uri(species.evolution_chain.url), cancellationToken);
                    var spriteId =
                        await mongoDbGridFsService.InsertAsync(new Uri(pokemon.sprites.front_default),
                            $"{pokemonName.Value}-sprite");
                    var audioId = await mongoDbGridFsService.InsertAsync(
                        new Uri(pokemon.cries.legacy ?? pokemon.cries.latest),
                        $"{pokemonName.Value}-audio");
                    await mongoDbService.InsertAsync(
                        ApiMapper.ToDocument(pokemon, species, evolutionChain, spriteId, audioId),
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