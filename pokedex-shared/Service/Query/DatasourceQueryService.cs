using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using pokedex_shared.Extension;
using pokedex_shared.Model.Document;
using pokedex_shared.Model.Document.Projection;
using pokedex_shared.Model.Domain;

namespace pokedex_shared.Service.Query;

/**
* <summary>
* Cache-Aside pattern with Redis and MongoDB
* </summary>
*/
public class DatasourceQueryService(
    ILogger<DatasourceQueryService> logger,
    RedisService redis,
    MongoDbQueryService mongoDbQueryService,
    MongoDbGridFsQueryService mongoDbGridFsQueryService)
{
    private const string CacheKeyPrefixPokemonId = "id:";
    private const string CacheKeyPrefixName = "name:";

    private readonly DistributedCacheEntryOptions _distributedCacheEntryOptions = new()
    {
        SlidingExpiration = TimeSpan.FromMinutes(10),
    };

    public async Task<PokemonDocument> FindByPokemonIdAsync(PokemonId pokemonId,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = CacheKeyPrefixPokemonId + pokemonId.Value;
        var cacheValue = await redis.GetAsync<PokemonDocument>(cacheKey, cancellationToken);
        if (cacheValue != default)
        {
            return cacheValue;
        }

        var db = await mongoDbQueryService.FindOneByPokemonIdAsync(pokemonId, cancellationToken);
        if (db == default)
        {
            return default;
        }

        var json = await db.ToJsonValueTypeAsync(cancellationToken);
        await redis.SetAsync(cacheKey, json, _distributedCacheEntryOptions, cancellationToken);
        return db;
    }

    public async Task<PokemonDocument> FindByNameAsync(PokemonName pokemonName,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = CacheKeyPrefixName + pokemonName.Value;
        var cacheValue = await redis.GetAsync<PokemonDocument>(cacheKey, cancellationToken);
        if (cacheValue != default)
        {
            return cacheValue;
        }

        var db = await mongoDbQueryService.FindOneByNameAsync(pokemonName, cancellationToken);
        if (db == default)
        {
            return default;
        }

        var json = await db.ToJsonValueTypeAsync(cancellationToken);
        await redis.SetAsync(cacheKey, json, _distributedCacheEntryOptions, cancellationToken);
        return db;
    }

    public async Task<List<PokemonMediaProjection>> SearchByNameAsync(PokemonName search,
        CancellationToken cancellationToken = default)
    {
        return await mongoDbQueryService.SearchByNameAsync(search, cancellationToken);
    }

    public async Task<List<PokemonMediaProjection>> FindAllByPokemonIdAsync(
        PokemonIdCollection pokemonIdCollection,
        CancellationToken cancellationToken = default)
    {
        return await mongoDbQueryService.FindAllByPokemonIdAsync(pokemonIdCollection, cancellationToken);
    }

    public async Task<PaginationResultDocument> FindAllAsync(int page, int pageSize,
        CancellationToken cancellationToken = default)
    {
        return await mongoDbQueryService.FindAllAsync(page, pageSize, cancellationToken);
    }

    public async Task<List<PokemonMediaProjection>> SearchByGenerationAsync(PokemonGeneration generation,
        CancellationToken cancellationToken)
    {
        return await mongoDbQueryService.SearchByGenerationAsync(generation, cancellationToken);
    }

    public async Task<PokemonFileResult?> FindFileByIdAsync(ObjectId id, CancellationToken cancellationToken = default)
    {
        return await mongoDbGridFsQueryService.FindFileByIdAsync(id, cancellationToken);
    }
}