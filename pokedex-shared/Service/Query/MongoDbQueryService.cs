using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using pokedex_shared.Model.Document;
using pokedex_shared.Model.Document.Projection;
using pokedex_shared.Model.Domain;
using pokedex_shared.Option;
using static MongoDB.Driver.Builders<pokedex_shared.Model.Document.PokemonDocument>;

namespace pokedex_shared.Service.Query;

public class MongoDbQueryService
{
    private const string CaseInsensitiveMatching = "i";
    private readonly ILogger<MongoDbQueryService> _logger;
    private readonly IMongoCollection<PokemonDocument> _collection;

    public MongoDbQueryService(
        ILogger<MongoDbQueryService> logger,
        IOptions<MongoDbOption> mongoDbOption)
    {
        _logger = logger;
        var mongoDb = mongoDbOption.Value;
        _collection = new MongoClient(mongoDb.ConnectionString)
            .GetDatabase(mongoDb.Database)
            .GetCollection<PokemonDocument>(mongoDb.Collection);
        _collection.Indexes.CreateManyAsync(CreateIndexes(["name", "generation"]));
    }

    private static List<CreateIndexModel<PokemonDocument>> CreateIndexes(string[] fieldNames)
    {
        return fieldNames
            .Select(fieldName => IndexKeys.Ascending(fieldName))
            .Select(definition => new CreateIndexModel<PokemonDocument>(definition)).ToList();
    }

    public async Task<PokemonDocument> FindOneByPokemonIdAsync(PokemonId pokemonId,
        CancellationToken cancellationToken = default)
    {
        return await _collection
            .Find(doc => doc.PokemonId == pokemonId.Value)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public async Task<PokemonDocument> FindOneByNameAsync(PokemonName pokemonName,
        CancellationToken cancellationToken = default)
    {
        return await _collection
            .Find(doc => doc.Name == pokemonName.Value)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public async Task<List<PokemonMediaProjectionDocument>> SearchByNameAsync(PokemonName search,
        CancellationToken cancellationToken = default)
    {
        var filter = Filter.Regex(
            doc => doc.Name,
            new BsonRegularExpression(search.Value, CaseInsensitiveMatching)
        );
        var projection = Projection
            .Include(doc => doc.Name)
            .Include(doc => doc.Images);
        return await _collection
            .Find(filter)
            .Project<PokemonMediaProjectionDocument>(projection)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<List<PokemonMediaProjectionDocument>> FindAllAsync(CancellationToken cancellationToken = default)
    {
        var projection = Projection
            .Include(doc => doc.Name)
            .Include(doc => doc.Images);
        return await _collection
            .Find(FilterDefinition<PokemonDocument>.Empty)
            .Project<PokemonMediaProjectionDocument>(projection)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<List<PokemonMediaProjectionDocument>> FindAllByPokemonIdAsync(
        PokemonIdCollection pokemonIdCollection,
        CancellationToken cancellationToken = default)
    {
        var ids = pokemonIdCollection.Ids
            .Select(id => id.Value)
            .ToList();
        var filter = Filter.In(doc => doc.PokemonId, ids);
        var projection = Projection
            .Include(doc => doc.Name)
            .Include(doc => doc.Images);
        return await _collection
            .Find(filter)
            .Project<PokemonMediaProjectionDocument>(projection)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<List<PokemonMediaProjectionDocument>> SearchByGenerationAsync(PokemonGeneration generation,
        CancellationToken cancellationToken)
    {
        var projection = Projection
            .Include(doc => doc.Name)
            .Include(doc => doc.Images);
        var filter = Filter.Where(doc => doc.Generation == generation.Value);
        return await _collection
            .Find(filter)
            .Project<PokemonMediaProjectionDocument>(projection)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}