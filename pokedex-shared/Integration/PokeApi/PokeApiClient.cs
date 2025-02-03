using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using pokedex_shared.Common;
using pokedex_shared.Service;

namespace pokedex_shared.Integration.PokeApi;

public class PokeApiClient(
    ILogger<PokeApiClient> logger,
    HttpClient httpClient,
    RedisService redis)
{
    private const string CacheKeyPrefix = "pokeapi.co:pokemon:";

    private readonly DistributedCacheEntryOptions _options = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(3)
    };

    public async Task<T> GetAsync<T>(Uri uri, CancellationToken cancellationToken = default) where T : struct
    {
        var cacheKey = uri.IsAbsoluteUri
            ? string.Concat(CacheKeyPrefix, uri.AbsolutePath)
            : string.Concat(CacheKeyPrefix, uri);
        var cacheValue = await redis.GetValueTypeAsync<T>(cacheKey, cancellationToken);
        if (!Equals(cacheValue, default(T)))
        {
            return cacheValue;
        }

        var json = await httpClient.GetStringAsync(uri, cancellationToken);
        await redis.SetAsync(cacheKey, json, _options, cancellationToken);
        return await json.DeserializeValueTypeToJsonAsync<T>(cancellationToken);
    }
}