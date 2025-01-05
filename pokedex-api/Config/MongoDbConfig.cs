



using pokedex_api.Service;

namespace pokedex_api.Config;

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