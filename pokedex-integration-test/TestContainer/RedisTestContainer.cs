using Testcontainers.Redis;

namespace pokedex_integration_test.TestContainer;

public class RedisTestContainer
{
    private RedisContainer? _container;

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        _container = new RedisBuilder().Build();
        await _container.StartAsync(cancellationToken);
    }

    public async Task DisposeAsync(CancellationToken cancellationToken = default)
    {
        await _container!.StopAsync(cancellationToken);
        await _container!.DisposeAsync();
    }

    public string GetConnectionString()
    {
        return _container!.GetConnectionString();
    }
}