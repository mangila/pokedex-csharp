using System.Net;
using pokedex_poller.Http;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;

namespace pokedex_poller.Config;

public static class PokemonConfig
{
    public static IServiceCollection AddPokemonApi(this IServiceCollection services,
        IConfigurationSection section)
    {
        var pokeOptions = section.Get<PokeApiOption>();
        services.AddHttpClient<PokemonClient>(client =>
            {
                client.BaseAddress = new Uri(pokeOptions.Url);
                client.Timeout = TimeSpan.FromMinutes(1);
            })
            .AddPolicyHandler(GetRetryPolicy())
            .SetHandlerLifetime(TimeSpan.FromMinutes(5));
        return services;
    }

    private static AsyncRetryPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
            .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                retryAttempt)));
    }
}