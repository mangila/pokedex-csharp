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
    private readonly string _cacheKeyPrefix = "pokeapi.co:";

    public async Task<T> GetAsync<T>(Uri uri, CancellationToken cancellationToken = default) where T : struct
    {
        string cacheKey = uri.IsAbsoluteUri ? _cacheKeyPrefix + uri.AbsolutePath : _cacheKeyPrefix + uri;
        var dto = await redis.GetAsync<T>(cacheKey, cancellationToken);
        if (dto.HasValue)
        {
            return dto.Value;
        }

        var json = await httpClient.GetStringAsync(uri, cancellationToken);
        await redis.SetAsync(cacheKey, json, new DistributedCacheEntryOptions(), cancellationToken);
        return JsonSerializer.Deserialize<T>(json, JsonConfig.JsonOptions);
    }
}