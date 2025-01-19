using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using pokedex_shared.Http;
using pokedex_shared.Option;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;

namespace pokedex_shared.Config;

public static class PokemonHttpClientConfig
{
    public static IServiceCollection AddPokemonApi(this IServiceCollection services,
        IConfigurationSection section)
    {
        var pokeOptions = section.Get<PokeApiOption>();
        services.AddHttpClient<PokemonHttpClient>(client =>
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
            .OrResult(msg => !msg.IsSuccessStatusCode)
            .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                retryAttempt)));
    }
}