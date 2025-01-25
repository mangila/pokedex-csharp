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
    private readonly string _cacheKeyPrefixPokemonId = "id:";
    private readonly string _cacheKeyPrefixName = "name:";

    private readonly DistributedCacheEntryOptions _distributedCacheEntryOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
    };

    public async Task<PokemonDto?> FindByPokemonIdAsync(PokemonId pokemonId,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = _cacheKeyPrefixPokemonId + pokemonId.Value;
        var cacheValue = await redis.GetAsync<PokemonDto>(cacheKey, cancellationToken);
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

    public async Task<PokemonDto?> FindByNameAsync(PokemonName pokemonName,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = _cacheKeyPrefixName + pokemonName.Value;
        var cacheValue = await redis.GetAsync<PokemonDto>(cacheKey, cancellationToken);
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

    public async Task<PokemonDtoCollection> SearchByNameAsync(PokemonName search,
        CancellationToken cancellationToken = default)
    {
        return await mongoDbQueryService.SearchByNameAsync(search, cancellationToken);
    }

    public async Task<PokemonDtoCollection> FindAllByPokemonIdAsync(PokemonIdCollection pokemonIdCollection,
        CancellationToken cancellationToken = default)
    {
        return await mongoDbQueryService.FindAllByPokemonIdAsync(pokemonIdCollection, cancellationToken);
    }

    public async Task<PokemonDtoCollection> FindAllAsync(CancellationToken cancellationToken = default)
    {
        return await mongoDbQueryService.FindAllAsync(cancellationToken);
    }

    public async Task<PokemonFileResult?> FindFileByIdAsync(ObjectId id, CancellationToken cancellationToken = default)
    {
        return await mongoDbGridFsQueryService.FindFileByIdAsync(id, cancellationToken);
    }

    public async Task<PokemonDtoCollection> SearchByGenerationAsync(PokemonGeneration generation,
        CancellationToken cancellationToken)
    {
        return await mongoDbQueryService.SearchByGenerationAsync(generation, cancellationToken);
    }
}