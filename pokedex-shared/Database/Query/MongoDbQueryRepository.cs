using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using pokedex_shared.Common.Option;
using pokedex_shared.Model.Document;
using pokedex_shared.Model.Document.Embedded;
using pokedex_shared.Model.Domain;
using static MongoDB.Driver.Builders<pokedex_shared.Model.Document.Embedded.PokemonDocument>;

namespace pokedex_shared.Database.Query;

public class MongoDbQueryRepository
{
    private const string CaseInsensitiveMatching = "i";
    private readonly ILogger<MongoDbQueryRepository> _logger;
    private readonly IMongoCollection<PokemonDocument> _collection;

    public MongoDbQueryRepository(
        ILogger<MongoDbQueryRepository> logger,
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
            .Select(definition => new CreateIndexModel<PokemonDocument>(definition))
            .ToList();
    }

    public async Task<PokemonDocument> FindOneByPokemonIdAsync(PokemonId pokemonId,
        CancellationToken cancellationToken = default)
    {
        return await _collection
            .Find(doc => doc.PokemonId.ToString() == pokemonId.Value)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public async Task<PokemonDocument> FindOneByNameAsync(PokemonName pokemonName,
        CancellationToken cancellationToken = default)
    {
        return await _collection
            .Find(doc => doc.Name == pokemonName.Value)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public async Task<List<PokemonMediaProjection>> SearchByNameAsync(PokemonName search,
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
            .Project<PokemonMediaProjection>(projection)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<List<PokemonMediaProjection>> FindAllAsync(CancellationToken cancellationToken = default)
    {
        var projection = Projection
            .Include(doc => doc.Name)
            .Include(doc => doc.Images);
        return await _collection
            .Find(FilterDefinition<PokemonDocument>.Empty)
            .Project<PokemonMediaProjection>(projection)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    /**
     * <summary>
     *  Pagination logic
     *  1. Get total documents
     *  2. Total / PageSize = To get the total pages to Paginate with
     *  3. Skip documents with -1 (zero indexed)
     *  4. Limit with the page size
     *  5. Sort ascending on PokemonId
     *  Might be more performance with range query, but then we need the boundary value.
     * </summary>
     */
    public async Task<PaginationResultDocument> FindAllAsync(int page, int pageSize,
        CancellationToken cancellationToken = default)
    {
        var count = await _collection.CountDocumentsAsync(
            FilterDefinition<PokemonDocument>.Empty,
            null,
            cancellationToken);

        var totalPages = (int)Math.Ceiling((double)count / pageSize);

        var documents = await _collection
            .Find(FilterDefinition<PokemonDocument>.Empty)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .Sort(Sort.Ascending(p => p.PokemonId))
            .ToListAsync(cancellationToken);

        return new PaginationResultDocument
        {
            TotalCount = count,
            TotalPages = totalPages,
            CurrentPage = page,
            PageSize = pageSize,
            Documents = documents
        };
    }


    public async Task<List<PokemonMediaProjection>> FindAllByPokemonIdAsync(
        PokemonIdCollection pokemonIdCollection,
        CancellationToken cancellationToken = default)
    {
        var ids = pokemonIdCollection.Ids
            .Select(id => id.Value)
            .ToList();
        var filter = Filter
            .In(doc => doc.PokemonId.ToString(), ids);
        var projection = Projection
            .Include(doc => doc.Name)
            .Include(doc => doc.Images);
        return await _collection
            .Find(filter)
            .Project<PokemonMediaProjection>(projection)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<List<PokemonMediaProjection>> SearchByGenerationAsync(PokemonGeneration generation,
        CancellationToken cancellationToken)
    {
        var projection = Projection
            .Include(doc => doc.Name)
            .Include(doc => doc.Images);
        var filter = Filter
            .Where(doc => doc.Generation == generation.Value);
        return await _collection
            .Find(filter)
            .Sort(Sort.Ascending(p => p.PokemonId))
            .Project<PokemonMediaProjection>(projection)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}