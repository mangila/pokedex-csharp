using Microsoft.Extensions.Caching.Distributed;

namespace pokedex_api.Service;

/**
* <summary>
* Cache-A-side pattern with Redis and MongoDB
* </summary>
*/
public class DatasourceService(
    ILogger<DatasourceService> logger,
    IDistributedCache redis,
    MongoDbService mongoDbService)
{
    public void FindAsync(string key, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}