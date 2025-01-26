using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using pokedex_shared.Extension;
using pokedex_shared.Model.Domain;
using pokedex_shared.Model.Dto;

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

    public async Task<PokemonDetailedDto?> FindByPokemonIdAsync(PokemonId pokemonId,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = CacheKeyPrefixPokemonId + pokemonId.Value;
        var cacheValue = await redis.GetAsync<PokemonDetailedDto>(cacheKey, cancellationToken);
        if (cacheValue is not null)
        {
            return cacheValue;
        }

        var db = await mongoDbQueryService.FindOneByPokemonIdAsync(pokemonId, cancellationToken);
        if (!db.HasValue)
        {
            return db;
        }

        var json = await db.ToJsonAsync(cancellationToken);
        await redis.SetAsync(cacheKey, json, _distributedCacheEntryOptions, cancellationToken);
        return db;
    }

    public async Task<PokemonDetailedDto?> FindByNameAsync(PokemonName pokemonName,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = CacheKeyPrefixName + pokemonName.Value;
        var cacheValue = await redis.GetAsync<PokemonDetailedDto>(cacheKey, cancellationToken);
        if (cacheValue is not null)
        {
            return cacheValue;
        }

        var db = await mongoDbQueryService.FindOneByNameAsync(pokemonName, cancellationToken);
        if (!db.HasValue)
        {
            return db;
        }

        var json = await db.ToJsonAsync(cancellationToken);
        await redis.SetAsync(cacheKey, json, _distributedCacheEntryOptions, cancellationToken);
        return db;
    }

    public async Task<PokemonNameImagesDtoCollection> SearchByNameAsync(PokemonName search,
        CancellationToken cancellationToken = default)
    {
        return await mongoDbQueryService.SearchByNameAsync(search, cancellationToken);
    }

    public async Task<PokemonNameImagesDtoCollection> FindAllByPokemonIdAsync(PokemonIdCollection pokemonIdCollection,
        CancellationToken cancellationToken = default)
    {
        return await mongoDbQueryService.FindAllByPokemonIdAsync(pokemonIdCollection, cancellationToken);
    }

    public async Task<PokemonNameImagesDtoCollection> FindAllAsync(CancellationToken cancellationToken = default)
    {
        return await mongoDbQueryService.FindAllAsync(cancellationToken);
    }

    public async Task<PokemonFileResult?> FindFileByIdAsync(ObjectId id, CancellationToken cancellationToken = default)
    {
        return await mongoDbGridFsQueryService.FindFileByIdAsync(id, cancellationToken);
    }

    public async Task<PokemonNameImagesDtoCollection> SearchByGenerationAsync(PokemonGeneration generation,
        CancellationToken cancellationToken)
    {
        return await mongoDbQueryService.SearchByGenerationAsync(generation, cancellationToken);
    }
}