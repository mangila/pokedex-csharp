using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using pokedex_shared.Config;

namespace pokedex_shared.Http;

public class PokemonHttpClient(
    ILogger<PokemonHttpClient> logger,
    HttpClient httpClient,
    IDistributedCache redis)
{
    private readonly DistributedCacheEntryOptions _distributedCacheEntryOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1), // Expire in 1 hour
    };

    public async Task<T> GetAsync<T>(Uri uri, CancellationToken cancellationToken = default)
    {
        var cacheKey = uri.ToString();
        var cacheValue = await redis.GetStringAsync(cacheKey, cancellationToken);
        if (cacheValue is null)
        {
            logger.LogInformation("Cache miss - {cacheKey}", cacheKey);
            var json = await httpClient.GetStringAsync(uri, cancellationToken);
            await redis.SetStringAsync(cacheKey, json, _distributedCacheEntryOptions, cancellationToken);
            return JsonSerializer.Deserialize<T>(json) ?? throw new InvalidOperationException();
        }

        logger.LogInformation("Cache hit - {cacheKey}", cacheKey);
        return JsonSerializer.Deserialize<T>(cacheValue, JsonConfig.JsonOptions) ??
               throw new InvalidOperationException();
    }
}