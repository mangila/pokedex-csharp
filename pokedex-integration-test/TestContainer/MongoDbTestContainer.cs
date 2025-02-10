using Testcontainers.MongoDb;

namespace pokedex_integration_test.TestContainer;

public class MongoDbTestContainer
{
    private MongoDbContainer? _container;

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        _container = new MongoDbBuilder()
            .WithUsername("admin")
            .WithPassword("password")
            .WithPortBinding(1337, 27017)
            .Build();
        await _container.StartAsync(cancellationToken);
    }

    public async Task DisposeAsync(CancellationToken cancellationToken = default)
    {
        await _container!.StopAsync(cancellationToken);
        await _container!.DisposeAsync();
    }
}