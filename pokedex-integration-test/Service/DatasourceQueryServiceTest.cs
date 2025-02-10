using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using pokedex_integration_test.TestContainer;
using pokedex_shared.Common.Option;
using pokedex_shared.Database;
using pokedex_shared.Model.Document;
using pokedex_shared.Model.Domain;
using pokedex_shared.Service;

namespace pokedex_integration_test.Service;

[TestFixture]
[TestOf(typeof(DatasourceQueryService))]
public class DatasourceQueryServiceTest
{
    private ServiceProvider _serviceProvider;
    private MongoDbTestContainer _mongoDbTestContainer = new();
    private RedisTestContainer _redisTestContainer = new();

    [SetUp]
    public async Task Setup()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Test.json", optional: false, reloadOnChange: false)
            .AddEnvironmentVariables()
            .Build();
        await _mongoDbTestContainer.InitializeAsync();
        await _redisTestContainer.InitializeAsync();
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = _redisTestContainer.GetConnectionString();
            options.InstanceName = "test";
        });
        serviceCollection.AddMongoDbQueryRepository(config.GetRequiredSection(nameof(MongoDbOption)));
        serviceCollection.AddSingleton<DatasourceQueryService>();
        serviceCollection.AddSingleton<RedisService>();
        serviceCollection.AddSingleton<ILogger<DatasourceQueryService>>(sp =>
        {
            var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
            return loggerFactory.CreateLogger<DatasourceQueryService>();
        });

        // Add logging
        serviceCollection.AddLogging(configure => configure.AddConsole());
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }

    [TearDown]
    public async Task Teardown()
    {
        await _mongoDbTestContainer.DisposeAsync();
        await _redisTestContainer.DisposeAsync();
        await _serviceProvider.DisposeAsync();
    }

    [Test]
    public async Task Test()
    {
        var service = _serviceProvider.GetService<DatasourceQueryService>();
        var document = await service!.FindByNameAsync(new PokemonName("name"), CancellationToken.None);
        document.Should()
            .NotBeNull()
            .And.BeEquivalentTo(default(PokemonSpeciesDocument));
    }

    [Test]
    public async Task Test2()
    {
        var service = _serviceProvider.GetService<DatasourceQueryService>();
        var document = await service!.FindByNameAsync(new PokemonName("name"), CancellationToken.None);
        document.Should()
            .NotBeNull()
            .And.BeEquivalentTo(default(PokemonSpeciesDocument));
    }
}