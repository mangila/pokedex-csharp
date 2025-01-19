using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using pokedex_shared.Config;
using pokedex_shared.Extension;
using pokedex_shared.Model.Domain;
using pokedex_shared.Model.Dto;

namespace pokedex_shared.Service;

/**
* <summary>
* Cache-A-side pattern with Redis and MongoDB
* </summary>
*/
public class DatasourceService(
    ILogger<DatasourceService> logger,
    IDistributedCache redis,
    MongoDbService mongoDbService)
{
    public async Task<PokemonDto?> FindByPokemonIdAsync(PokemonId pokemonId,
        CancellationToken cancellationToken = default)
    {
        var cacheValue = await redis.GetStringAsync(pokemonId.Value, cancellationToken);
        if (cacheValue is not null)
        {
            logger.LogDebug("Cache hit - {}", pokemonId);
            var dto = JsonSerializer.Deserialize<PokemonDto>(cacheValue, JsonConfig.JsonOptions);
            return dto;
        }

        logger.LogDebug("Cache miss - {}", pokemonId);
        var databaseValue = await mongoDbService.FindOneByPokemonIdAsync(pokemonId, cancellationToken);
        if (!databaseValue.HasValue)
        {
            return databaseValue;
        }

        var json = await databaseValue.Value.ToJsonAsync(cancellationToken);
        await redis.SetStringAsync(pokemonId.Value, json, token: cancellationToken);
        return databaseValue;
    }

    public async Task<PokemonDto?> FindByNameAsync(PokemonName pokemonName,
        CancellationToken cancellationToken = default)
    {
        var cacheValue = await redis.GetStringAsync(pokemonName.Value, cancellationToken);
        if (cacheValue is not null)
        {
            logger.LogDebug("Cache hit - {}", pokemonName);
            var dto = JsonSerializer.Deserialize<PokemonDto>(cacheValue, JsonConfig.JsonOptions);
            return dto;
        }

        logger.LogDebug("Cache miss - {}", pokemonName);
        var databaseValue = await mongoDbService.FindOneByNameAsync(pokemonName, cancellationToken);
        if (!databaseValue.HasValue)
        {
            return databaseValue;
        }

        var json = await databaseValue.Value.ToJsonAsync(cancellationToken);
        await redis.SetStringAsync(pokemonName.Value, json, token: cancellationToken);
        return databaseValue;
    }

    public async Task<PokemonDtoCollection> SearchByName(string search,
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