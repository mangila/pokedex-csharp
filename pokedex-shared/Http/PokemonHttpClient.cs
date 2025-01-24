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
    public async Task<T> GetAsync<T>(Uri uri, CancellationToken cancellationToken = default) where T : struct
    {
        var key = uri.ToString();
        var dto = await redis.GetAsync<T>(key, cancellationToken);
        if (dto.HasValue)
        {
            return dto.Value;
        }

        var json = await httpClient.GetStringAsync(uri, cancellationToken);
        await redis.SetAsync(key, json, new DistributedCacheEntryOptions(), cancellationToken);
        return JsonSerializer.Deserialize<T>(json, JsonConfig.JsonOptions);
    }
}