using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using pokedex_shared.Model.Document;
using pokedex_shared.Model.Domain;
using pokedex_shared.Model.Dto;
using pokedex_shared.Option;

namespace pokedex_shared.Service;

public class MongoDbService
{
    private const string CaseInsensitiveMatching = "i";
    private readonly ILogger<MongoDbService> _logger;
    private readonly IMongoCollection<PokemonDocument> _collection;

    public MongoDbService(
        ILogger<MongoDbService> logger,
        IOptions<MongoDbOption> mongoDbOption)
    {
        _logger = logger;
        var mongoDb = mongoDbOption.Value;
        _collection = new MongoClient(mongoDb.ConnectionString)
            .GetDatabase(mongoDb.Database)
            .GetCollection<PokemonDocument>(mongoDb.Collection);
        _collection.Indexes.CreateManyAsync(CreateIndexes(["pokemon_id", "name"]));
    }

    private static List<CreateIndexModel<PokemonDocument>> CreateIndexes(string[] fieldNames)
    {
        return fieldNames
            .Select(fieldName => Builders<PokemonDocument>.IndexKeys.Ascending(fieldName))
            .Select(definition => new CreateIndexModel<PokemonDocument>(definition)).ToList();
    }

    public async Task<PokemonDto?> FindOneByPokemonIdAsync(PokemonId pokemonId,
        CancellationToken cancellationToken = default)
    {
        using var cursor = await _collection.FindAsync(pokemonDocument => pokemonDocument.PokemonId == pokemonId.Value,
            cancellationToken: cancellationToken);
        var document = await cursor.FirstOrDefaultAsync(cancellationToken);
        return document?.ToDto();
    }

    public async Task<PokemonDto?> FindOneByNameAsync(PokemonName pokemonName,
        CancellationToken cancellationToken = default)
    {
        using var cursor = await _collection.FindAsync(pokemonDocument => pokemonDocument.Name == pokemonName.Value,
            cancellationToken: cancellationToken);
        var document = await cursor.FirstOrDefaultAsync(cancellationToken);
        return document?.ToDto();
    }

    public async Task UpsertAsync(PokemonDocument pokemon, CancellationToken cancellationToken = default)
    {
        await _collection.ReplaceOneAsync(
            doc => doc.PokemonId == pokemon.PokemonId,
            pokemon,
            new ReplaceOptions
            {
                IsUpsert = true,
                Comment = "Insert from UpsertAsync() - upsert a new Pokemon"
            },
            cancellationToken: cancellationToken);
    }

    public async Task<PokemonDtoCollection> SearchByNameAsync(PokemonName search,
        CancellationToken cancellationToken = default)
    {
        var filter = Builders<PokemonDocument>.Filter.Regex(
            doc => doc.Name,
            new BsonRegularExpression(search.Value, CaseInsensitiveMatching)
        );

        using var cursor = await _collection.FindAsync(filter, cancellationToken: cancellationToken);
        return await GetPokemonDtoCollectionAsync(cursor, cancellationToken);
    }

    public async Task<PokemonDtoCollection> FindAllByPokemonIdAsync(PokemonIdCollection pokemonIdCollection,
        CancellationToken cancellationToken = default)
    {
        var ids = pokemonIdCollection.Ids.Select(id => id.Value).ToList();
        var filter = Builders<PokemonDocument>.Filter.In(doc => doc.PokemonId, ids);
        using var cursor = await _collection.FindAsync(filter, cancellationToken: cancellationToken);
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