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
    MongoDbQueryService mongoDbCommandService,
    MongoDbGridFsQueryService mongoDbGridFsCommandService)
{
    private readonly DistributedCacheEntryOptions _distributedCacheEntryOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1), // Expire in 1 hour
    };

    public async Task<PokemonDto?> FindByPokemonIdAsync(PokemonId pokemonId,
        CancellationToken cancellationToken = default)
    {
        var cache = await redis.GetAsync<PokemonDto>(pokemonId.Value, cancellationToken);
        if (cache is not null)
        {
            return cache;
        }

        var db = await mongoDbCommandService.FindOneByPokemonIdAsync(pokemonId, cancellationToken);
        if (!db.HasValue)
        {
            return db;
        }

        var json = await db.ToJsonAsync(cancellationToken);
        await redis.SetAsync(pokemonId.Value, json, _distributedCacheEntryOptions, cancellationToken);
        return db;
    }

    public async Task<PokemonDto?> FindByNameAsync(PokemonName pokemonName,
        CancellationToken cancellationToken = default)
    {
        var cache = await redis.GetAsync<PokemonDto>(pokemonName.Value, cancellationToken);
        if (cache is not null)
        {
            return cache;
        }

        var db = await mongoDbCommandService.FindOneByNameAsync(pokemonName, cancellationToken);
        if (!db.HasValue)
        {
            return db;
        }

        var json = await db.ToJsonAsync(cancellationToken);
        await redis.SetAsync(pokemonName.Value, json, _distributedCacheEntryOptions, cancellationToken);
        return db;
    }

    public async Task<PokemonDtoCollection> SearchByNameAsync(PokemonName search,
        CancellationToken cancellationToken = default)
    {
        return await mongoDbCommandService.SearchByNameAsync(search, cancellationToken);
    }

    public async Task<PokemonDtoCollection> FindAllByPokemonIdAsync(PokemonIdCollection pokemonIdCollection,
        CancellationToken cancellationToken = default)
    {
        return await mongoDbCommandService.FindAllByPokemonIdAsync(pokemonIdCollection, cancellationToken);
    }

    public async Task<PokemonDtoCollection> FindAllAsync(CancellationToken cancellationToken = default)
    {
        return await mongoDbCommandService.FindAllAsync(cancellationToken);
    }

    public async Task<PokemonFileResult?> FindFileByIdAsync(ObjectId id, CancellationToken cancellationToken = default)
    {
        return await mongoDbGridFsCommandService.FindFileByIdAsync(id, cancellationToken);
    }
}