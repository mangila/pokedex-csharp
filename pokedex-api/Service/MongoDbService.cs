using Microsoft.Extensions.Options;
using MongoDB.Driver;
using pokedex_api.Config;
using pokedex_api.Model;

namespace pokedex_api.Service;

public class MongoDbService
{
    private readonly ILogger<MongoDbService> _logger;
    private readonly IMongoCollection<PokemonResponse> _collection;

    public MongoDbService(
        ILogger<MongoDbService> logger,
        IOptions<MongoDbOption> mongoDbOption)
    {
        _logger = logger;
        var options = mongoDbOption.Value;
        var client = new MongoClient(options.ConnectionString);
        _collection = client
            .GetDatabase(options.Database)
            .GetCollection<PokemonResponse>(options.Collection);
    }

    public Task<PokemonResponse> FindByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<PokemonResponse> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}