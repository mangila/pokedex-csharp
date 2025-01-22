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
            logger.LogInformation("worker started fetch - : {pokemonGeneration}", pokemonGeneration);
            try
            {
                var generation =
                    await pokemonHttpClient.Get<PokemonGenerationApiResponse>(GetPokemonGenerationUri(),
                        cancellationToken);
                foreach (var generationPokemon in generation.pokemon_species)
                {
                    logger.LogInformation("Poll: {name}", generationPokemon.name);
                    var pokemon = await pokemonHttpClient.Get<PokemonApiResponse>(
                        GetPokemonRelativeUri(generationPokemon.name),
                        cancellationToken);
                    var species = await pokemonHttpClient.Get<SpeciesApiResponse>(new Uri(pokemon.species.url),
                        cancellationToken);
                    var evolutionChain = await pokemonHttpClient.Get<EvolutionChainApiResponse>(
                        new Uri(species.evolution_chain.url), cancellationToken);
                    var spriteId =
                        await mongoDbGridFsService.InsertAsync(new Uri(pokemon.sprites.front_default),
                            pokemon.name + "-sprite");
                    var audioId = await mongoDbGridFsService.InsertAsync(
                        new Uri(pokemon.cries.legacy ?? pokemon.cries.latest),
                        pokemon.name + "-audio");
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

    private Uri GetPokemonGenerationUri()
    {
        int value = (int)pokemonGeneration;
        return new Uri(
            $"{pokeApiOption.GetPokemonGenerationUri}"
                .Replace("{id}", value.ToString())
            , UriKind.Relative);
    }

    private Uri GetPokemonRelativeUri(string name)
    {
        return new Uri(
            $"{pokeApiOption.GetPokemonUri}".Replace("{id}", name)
            , UriKind.Relative);
    }
}