using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using pokedex_shared.Config;
using pokedex_shared.Service;

namespace pokedex_shared.Http;

public class PokemonHttpClient(
    ILogger<PokemonHttpClient> logger,
    HttpClient httpClient,
    RedisService redis)
{
    private const string CacheKeyPrefix = "pokeapi.co:";

    private readonly DistributedCacheEntryOptions _options = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(3)
    };

    public async Task<T> GetAsync<T>(Uri uri, CancellationToken cancellationToken = default) where T : struct
    {
        var cacheKey = uri.IsAbsoluteUri ? CacheKeyPrefix + uri.AbsolutePath : CacheKeyPrefix + uri;
        var cacheValue = await redis.GetAsync<T>(cacheKey, cancellationToken);
        if (!Equals(cacheValue, default(T)))
        {
            return cacheValue;
        }

        var json = await httpClient.GetStringAsync(uri, cancellationToken);
        await redis.SetAsync(cacheKey, json, _options, cancellationToken);
        return JsonSerializer.Deserialize<T>(json, JsonConfig.JsonOptions);
    }
}