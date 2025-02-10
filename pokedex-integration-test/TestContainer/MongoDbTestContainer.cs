using Testcontainers.MongoDb;

namespace pokedex_integration_test.TestContainer;

public class MongoDbTestContainer
{
    private MongoDbContainer? _container;

    public async Task InitializeAsync()
    {
        _container = new MongoDbBuilder()
            .WithUsername("admin")
            .WithPassword("password")
            .WithPortBinding(1337, 27017)
            .Build();
        await _container.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _container!.StopAsync();
        await _container!.DisposeAsync();
    }
}