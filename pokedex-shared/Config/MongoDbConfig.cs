using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using pokedex_shared.Option;
using pokedex_shared.Service;
using pokedex_shared.Service.Command;
using pokedex_shared.Service.Query;

namespace pokedex_shared.Config;

public static class MongoDbConfig
{
    public static IServiceCollection AddMongoDbQueryService(this IServiceCollection services,
        IConfigurationSection section)
    {
        services.AddOptions<MongoDbOption>()
            .Bind(section)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        services.AddSingleton<MongoDbQueryService>();
        services.AddSingleton<MongoDbGridFsQueryService>();
        return services;
    }

    public static IServiceCollection AddMongoDbCommandService(this IServiceCollection services,
        IConfigurationSection section)
    {
        services.AddOptions<MongoDbOption>()
            .Bind(section)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        services.AddSingleton<MongoDbCommandService>();
        services.AddSingleton<MongoDbGridFsCommandService>();
        return services;
    }
}