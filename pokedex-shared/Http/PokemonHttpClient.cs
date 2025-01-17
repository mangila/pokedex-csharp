using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using pokedex_poller.Config;
using pokedex_shared.Model;

namespace pokedex_shared.Http;

public class PokemonHttpClient(
    ILogger<PokemonHttpClient> logger,
    HttpClient httpClient,
    IOptions<PokeApiOption> options)
{
    public async Task<PokemonApiResponse> GetPokemon(string id, CancellationToken cancellationToken = default)
    {
        var uri = options
            .Value
            .GetPokemonUri
            .Replace("{id}", id);
        return await httpClient.GetFromJsonAsync<PokemonApiResponse>(uri, cancellationToken);
    }
}