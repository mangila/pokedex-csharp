using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using pokedex_shared.Extension;
using pokedex_shared.Model.Domain;
using pokedex_shared.Model.Dto;

namespace pokedex_shared.Service;

/**
* <summary>
* Cache-Aside pattern with Redis and MongoDB
* </summary>
*/
public class DatasourceService(
    ILogger<DatasourceService> logger,
    RedisService redis,
    MongoDbService mongoDbService)
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

        var db = await mongoDbService.FindOneByPokemonIdAsync(pokemonId, cancellationToken);
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

        var db = await mongoDbService.FindOneByNameAsync(pokemonName, cancellationToken);
        if (!db.HasValue)
        {
            return db;
        }

        var json = await db.ToJsonAsync(cancellationToken);
        await redis.SetAsync(pokemonName.Value, json, _distributedCacheEntryOptions, cancellationToken);
        return db;
    }

    public async Task<PokemonDtoCollection> SearchByName(PokemonName search,
        CancellationToken cancellationToken = default)
    {
        return await mongoDbService.SearchByNameAsync(search, cancellationToken);
    }

    public async Task<PokemonDtoCollection> FindAllByPokemonIdAsync(PokemonIdCollection pokemonIdCollection,
        CancellationToken cancellationToken = default)
    {
        return await mongoDbService.FindAllByPokemonIdAsync(pokemonIdCollection, cancellationToken);
    }

    public async Task<PokemonDtoCollection> FindAllAsync(CancellationToken cancellationToken = default)
    {
        return await mongoDbService.FindAllAsync(cancellationToken);
    }
}