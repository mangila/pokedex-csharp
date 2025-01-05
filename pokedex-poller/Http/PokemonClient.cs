using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using pokedex_poller.Config;

namespace pokedex_poller.Http;

public class PokemonClient(
    ILogger<PokemonClient> logger,
    HttpClient httpClient,
    IOptions<PokeApiOption> options)
{
    public async Task<Pokemon?> GetPokemon(string id, CancellationToken cancellationToken = default)
    {
        var uri = options
            .Value
            .GetPokemonUri
            .Replace("{id}", id);
        return await httpClient.GetFromJsonAsync<Pokemon>(uri, cancellationToken);
    }
}