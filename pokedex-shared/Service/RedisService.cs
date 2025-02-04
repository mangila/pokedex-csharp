using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using pokedex_shared.Common;

namespace pokedex_shared.Service;

public class RedisService(
    ILogger<DatasourceQueryService> logger,
    IDistributedCache redis)
{
    public async Task<T> GetValueTypeAsync<T>(
        string key,
        CancellationToken cancellationToken = default) where T : struct
    {
        var cacheValue = await redis.GetStringAsync(key, cancellationToken);
        if (cacheValue is null)
        {
            logger.LogInformation("Cache miss - {key}", key);
            return default;
        }

        logger.LogInformation("Cache hit - {key}", key);

        return await cacheValue.DeserializeValueTypeToJsonAsync<T>(cancellationToken);
    }

    public async Task<byte[]?> GetAsync(
        string key,
        CancellationToken cancellationToken = default)
    {
        var cacheValue = await redis.GetAsync(key, cancellationToken);
        if (cacheValue is null)
        {
            logger.LogInformation("Cache miss - {key}", key);
            return null;
        }

        logger.LogInformation("Cache hit - {key}", key);

        return cacheValue;
    }

    public async Task SetStringAsync(
        string key,
        string json,
        DistributedCacheEntryOptions options,
        CancellationToken cancellationToken = default)
    {
        await redis.SetStringAsync(key, json, options, cancellationToken);
    }

    public async Task SetAsync(
        string key,
        byte[] data,
        DistributedCacheEntryOptions options,
        CancellationToken cancellationToken = default)
    {
        await redis.SetAsync(key, data, options, cancellationToken);
    }
}