using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using pokedex_shared.Model;
using pokedex_shared.Option;

namespace pokedex_shared.Service;

public class MongoDbService
{
    private readonly ILogger<MongoDbService> _logger;
    private readonly IMongoCollection<PokemonDocument> _collection;

    public MongoDbService(
        ILogger<MongoDbService> logger,
        IOptions<MongoDbOption> mongoDbOption)
    {
        _logger = logger;
        var options = mongoDbOption.Value;
        _collection = new MongoClient(options.ConnectionString)
            .GetDatabase(options.Database)
            .GetCollection<PokemonDocument>(options.Collection);
    }

    public async Task<PokemonDto?> FindOneByPokemonIdAsync(PokemonId pokemonId,
        CancellationToken cancellationToken = default)
    {
        using var cursor = await _collection.FindAsync(pokemonDocument => pokemonDocument.PokemonId == pokemonId.id,
            cancellationToken: cancellationToken);
        var document = await cursor.FirstOrDefaultAsync(cancellationToken);
        return document?.ToDto();
    }

    public async Task<PokemonDto?> FindOneByNameAsync(PokemonName pokemonName,
        CancellationToken cancellationToken = default)
    {
        using var cursor = await _collection.FindAsync(pokemonDocument => pokemonDocument.Name == pokemonName.name,
            cancellationToken: cancellationToken);
        var document = await cursor.FirstOrDefaultAsync(cancellationToken);
        return document?.ToDto();
    }

    public async Task InsertAsync(PokemonApiResponse pokemon, CancellationToken cancellationToken = default)
    {
        await _collection.InsertOneAsync(pokemon.ToDocument(), new InsertOneOptions
        {
            Comment = "Insert from InsertAsync()"
        }, cancellationToken);
    }

    public async Task<PokemonDtoCollection> SearchByNameAsync(string search,
        CancellationToken cancellationToken = default)
    {
        using var cursor = await _collection.FindAsync(document => document.Name.Contains(search),
            cancellationToken: cancellationToken);
        return await GetPokemonDtoCollectionAsync(cursor, cancellationToken);
    }

    public async Task<PokemonDtoCollection> FindAllByPokemonIdAsync(PokemonIdCollection pokemonIdCollection,
        CancellationToken cancellationToken = default)
    {
        using var cursor = await _collection.FindAsync(document =>
                pokemonIdCollection.Ids.Exists(id => document.PokemonId == id.id),
            cancellationToken: cancellationToken);
        return await GetPokemonDtoCollectionAsync(cursor, cancellationToken);
    }

    public async Task<PokemonDtoCollection> FindAllAsync(CancellationToken cancellationToken = default)
    {
        using var cursor = await _collection.FindAsync(FilterDefinition<PokemonDocument>.Empty,
            cancellationToken: cancellationToken);
        return await GetPokemonDtoCollectionAsync(cursor, cancellationToken);
    }

    private static async Task<PokemonDtoCollection> GetPokemonDtoCollectionAsync(IAsyncCursor<PokemonDocument> cursor,
        CancellationToken cancellationToken = default)
    {
        var collection = new PokemonDtoCollection();
        while (await cursor.MoveNextAsync(cancellationToken))
        {
            foreach (var document in cursor.Current)
            {
                collection.pokemons.Add(document.ToDto());
            }
        }

        return collection;
    }
}