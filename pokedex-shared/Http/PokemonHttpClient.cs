using System.Net.Http.Json;

namespace pokedex_shared.Http;

public class PokemonHttpClient(HttpClient httpClient)
{
    public async Task<T> Get<T>(Uri uri, CancellationToken cancellationToken = default)
    {
        return await httpClient.GetFromJsonAsync<T>(uri, cancellationToken) ??
               throw new InvalidOperationException("null response");
    }
}