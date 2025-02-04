using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using pokedex_shared.Common;
using pokedex_shared.Database.Query;
using pokedex_shared.Model.Document;
using pokedex_shared.Model.Domain;
using pokedex_shared.Model.Dto;

namespace pokedex_shared.Service;

/**
* <summary>
* Cache-Aside pattern with Redis and MongoDB
* </summary>
*/
public class DatasourceQueryService(
    ILogger<DatasourceQueryService> logger,
    RedisService redis,
    MongoDbQueryRepository mongoDbQueryRepository,
    MongoDbGridFsQueryRepository mongoDbGridFsQueryRepository)
{
    private const string CacheKeyPrefixPokemonId = "id:";
    private const string CacheKeyPrefixName = "name:";

    private readonly DistributedCacheEntryOptions _distributedCacheEntryOptions = new()
    {
        SlidingExpiration = TimeSpan.FromMinutes(10),
    };

    public async Task<PokemonSpeciesDocument> FindOneByIdAsync(
        PokemonId pokemonId,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = string.Concat(CacheKeyPrefixPokemonId, pokemonId.Value);
        var cacheValue = await redis.GetValueTypeAsync<PokemonSpeciesDocument>(cacheKey, cancellationToken);
        if (cacheValue != default)
        {
            return cacheValue;
        }

        var db = await mongoDbQueryRepository.FindOneByIdAsync(pokemonId, cancellationToken);
        if (db == default)
        {
            return default;
        }

        var json = await db.ToJsonValueTypeAsync(cancellationToken);
        await redis.SetStringAsync(cacheKey, json, _distributedCacheEntryOptions, cancellationToken);
        return db;
    }

    public async Task<PokemonSpeciesDocument> FindByNameAsync(
        PokemonName pokemonName,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = string.Concat(CacheKeyPrefixName, pokemonName.Value);
        var cacheValue = await redis.GetValueTypeAsync<PokemonSpeciesDocument>(cacheKey, cancellationToken);
        if (cacheValue != default)
        {
            return cacheValue;
        }

        var db = await mongoDbQueryRepository.FindOneByNameAsync(pokemonName, cancellationToken);
        if (db == default)
        {
            return default;
        }

        var json = await db.ToJsonValueTypeAsync(cancellationToken);
        await redis.SetStringAsync(cacheKey, json, _distributedCacheEntryOptions, cancellationToken);
        return db;
    }

    public async Task<List<PokemonSpeciesDocument>> SearchByNameAsync(
        PokemonName search,
        CancellationToken cancellationToken = default)
    {
        return await mongoDbQueryRepository.SearchByNameAsync(search, cancellationToken);
    }

    public async Task<List<PokemonSpeciesDocument>> FindAllByIdsAsync(
        PokemonIdCollection pokemonIdCollection,
        CancellationToken cancellationToken = default)
    {
        return await mongoDbQueryRepository.FindAllByIdsAsync(pokemonIdCollection, cancellationToken);
    }

    public async Task<PaginationResultDocument> FindByPaginationAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        return await mongoDbQueryRepository.FindByPaginationAsync(page, pageSize, cancellationToken);
    }

    public async Task<List<PokemonSpeciesDocument>> SearchByGenerationAsync(
        PokemonGeneration generation,
        CancellationToken cancellationToken)
    {
        return await mongoDbQueryRepository.SearchByGenerationAsync(generation, cancellationToken);
    }

    public async Task<PokemonFileResult?> FindFileByIdAsync(
        ObjectId id,
        CancellationToken cancellationToken = default)
    {
        return await mongoDbGridFsQueryRepository.FindFileByIdAsync(id, cancellationToken);
    }
}