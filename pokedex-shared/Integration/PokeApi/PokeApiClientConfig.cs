using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using pokedex_shared.Common.Option;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;

namespace pokedex_shared.Integration.PokeApi;

public static class PokeApiClientConfig
{
    public static IServiceCollection AddPokeApi(this IServiceCollection services,
        IConfigurationSection section)
    {
        services.AddOptions<PokeApiOption>()
            .Bind(section)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        var option = section.Get<PokeApiOption>();
        services.AddHttpClient<PokeApiClient>(client =>
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
            .GetService<ILogger<PokeApiClient>>();
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == HttpStatusCode.InternalServerError)
            .OrResult(msg => msg.StatusCode == HttpStatusCode.TooManyRequests)
            .WaitAndRetryAsync(6, retryAttempt =>
            {
                var delay = TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) +
                            TimeSpan.FromMilliseconds(new Random().Next(100, 500));
                logger?.LogWarning("Retrying in {delay} seconds", delay.TotalSeconds);
                return delay;
            });
    }
}