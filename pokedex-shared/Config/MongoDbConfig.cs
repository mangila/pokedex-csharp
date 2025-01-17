using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using pokedex_shared.Option;
using pokedex_shared.Service;

namespace pokedex_shared.Config;

public static class MongoDbConfig
{
    public static IServiceCollection AddMongoDb(this IServiceCollection services,
        IConfigurationSection section)
    {
        services.AddOptions<MongoDbOption>()
            .Bind(section)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        services.AddSingleton<MongoDbService>();
        return services;
    }
}