using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using pokedex_shared.Common.Option;
using pokedex_shared.Database.Command;
using pokedex_shared.Database.Query;

namespace pokedex_shared.Database;

public static class MongoDbConfig
{
    public static IServiceCollection AddMongoDbQueryRepository(this IServiceCollection services,
        IConfigurationSection section)
    {
        services.AddOptions<MongoDbOption>()
            .Bind(section)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        services.AddSingleton<MongoDbQueryRepository>();
        services.AddSingleton<MongoDbGridFsQueryRepository>();
        return services;
    }

    public static IServiceCollection AddMongoDbCommandRepository(this IServiceCollection services,
        IConfigurationSection section)
    {
        services.AddOptions<MongoDbOption>()
            .Bind(section)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        services.AddSingleton<MongoDbCommandRepository>();
        services.AddSingleton<MongoDbGridFsCommandRepository>();
        return services;
    }
}