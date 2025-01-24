using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using pokedex_shared.Config;

namespace pokedex_shared.Service;

public class RedisService(
    ILogger<DatasourceService> logger,
    IDistributedCache redis)
{
    public async Task<T?> GetAsync<T>(string key,
        CancellationToken cancellationToken = default) where T : struct
    {
        var cacheValue = await redis.GetStringAsync(key, cancellationToken);
        if (cacheValue is null)
        {
            logger.LogInformation("Cache miss - {key}", key);
            return null;
        }

        logger.LogInformation("Cache hit - {key}", key);
        return JsonSerializer.Deserialize<T>(cacheValue, JsonConfig.JsonOptions);
    }

    public async Task SetAsync(
        string key,
        string json,
        DistributedCacheEntryOptions options,
        CancellationToken cancellationToken = default)
    {
        await redis.SetStringAsync(key, json, options, cancellationToken);
    }
}