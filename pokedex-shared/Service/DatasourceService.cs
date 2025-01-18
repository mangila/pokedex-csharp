using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using pokedex_shared.Config;
using pokedex_shared.Model;

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
    public async Task<PokemonDto?> FindAsync(string key, CancellationToken cancellationToken = default)
    {
        var cacheValue = await redis.GetStringAsync(key, cancellationToken);
        if (cacheValue is null)
        {
            logger.LogDebug("Cache miss - {}", key);
            var databaseValue = await mongoDbService.FindByPokemonIdAsync(key, cancellationToken);
            if (databaseValue.HasValue)
            {
                var json = await databaseValue.Value.ToJsonAsync(cancellationToken);
                await redis.SetStringAsync(key, json, token: cancellationToken);
            }

            return databaseValue;
        }

        logger.LogDebug("Cache hit - {}", key);
        var dto = JsonSerializer.Deserialize<PokemonDto>(cacheValue, JsonConfig.JsonOptions);
        return dto;
    }
}