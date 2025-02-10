using Testcontainers.Redis;

namespace pokedex_integration_test.TestContainer;

public class RedisTestContainer
{
    private RedisContainer? _container;

    public async Task InitializeAsync()
    {
        _container = new RedisBuilder().Build();
        await _container.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _container!.StopAsync();
        await _container!.DisposeAsync();
    }

    public string GetConnectionString()
    {
        return _container!.GetConnectionString();
    }
}