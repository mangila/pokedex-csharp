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
    private readonly MongoDbTestContainer _mongoDbTestContainer = new();
    private readonly RedisTestContainer _redisTestContainer = new();
    private readonly CancellationTokenSource _cts = new();

    [SetUp]
    public async Task Setup()
    {
        var token = _cts.Token;
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Test.json", optional: false, reloadOnChange: false)
            .AddEnvironmentVariables()
            .Build();
        await _mongoDbTestContainer.InitializeAsync(token);
        await _redisTestContainer.InitializeAsync(token);
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = _redisTestContainer.GetConnectionString();
            options.InstanceName = "test";
        });
        serviceCollection.AddMongoDbQueryRepository(config.GetRequiredSection(nameof(MongoDbOption)));
        serviceCollection.AddSingleton<DatasourceQueryService>();
        serviceCollection.AddSingleton<RedisService>();
        serviceCollection.AddSingleton<ILogger<RedisService>>(sp =>
        {
            var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
            return loggerFactory.CreateLogger<RedisService>();
        });
        serviceCollection.AddLogging(configure => configure.AddConsole());
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }

    [TearDown]
    public async Task Teardown()
    {
        var token = _cts.Token;
        await _mongoDbTestContainer.DisposeAsync(token);
        await _redisTestContainer.DisposeAsync(token);
        await _serviceProvider.DisposeAsync();
    }

    [Test]
    public async Task Test()
    {
        var token = _cts.Token;
        var service = _serviceProvider.GetService<DatasourceQueryService>();
        var document = await service!.FindByNameAsync(new PokemonName("name"), token);
        document.Should()
            .NotBeNull()
            .And.BeEquivalentTo(default(PokemonSpeciesDocument));
    }

    [Test]
    public async Task Test2()
    {
        var token = _cts.Token;
        var service = _serviceProvider.GetService<DatasourceQueryService>();
        var document = await service!.FindByNameAsync(new PokemonName("name"), token);
        document.Should()
            .NotBeNull()
            .And.BeEquivalentTo(default(PokemonSpeciesDocument));
    }
}