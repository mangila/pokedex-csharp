using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
        services.AddOptions<PokeApiOption>()
            .Bind(section)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        var option = section.Get<PokeApiOption>();
        services.AddHttpClient<PokemonHttpClient>(client =>
            {
                client.BaseAddress = new Uri(option!.Url);
                client.Timeout = TimeSpan.FromMinutes(1);
            })
            .AddPolicyHandler(GetRetryPolicy(services))
            .SetHandlerLifetime(TimeSpan.FromMinutes(5));
        return services;
    }

    private static AsyncRetryPolicy<HttpResponseMessage> GetRetryPolicy(IServiceCollection services)
    {
        var logger = services.BuildServiceProvider()
            .GetService<ILogger<PokemonHttpClient>>();
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == HttpStatusCode.InternalServerError)
            .OrResult(msg => msg.StatusCode == HttpStatusCode.TooManyRequests)
            .WaitAndRetryAsync(6, retryAttempt =>
            {
                var delay = TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) +
                            TimeSpan.FromMilliseconds(new Random().Next(100, 500));
                logger?.LogWarning($"Retrying in {delay.TotalSeconds} seconds...");
                return delay;
            });
    }
}