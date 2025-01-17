using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using pokedex_shared.Model;
using pokedex_shared.Option;

namespace pokedex_shared.Service;

public class MongoDbService
{
    private readonly ILogger<MongoDbService> _logger;
    private readonly IMongoCollection<PokemonDocument> _collection;

    public MongoDbService(
        ILogger<MongoDbService> logger,
        IOptions<MongoDbOption> mongoDbOption)
    {
        _logger = logger;
        var options = mongoDbOption.Value;
        _collection = new MongoClient(options.ConnectionString)
            .GetDatabase(options.Database)
            .GetCollection<PokemonDocument>(options.Collection);
    }

    public Task<PokemonDocument> FindByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<PokemonDocument> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void InsertAsync(PokemonApiResponse pokemon, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(pokemon);
        _collection.InsertOne(pokemon.ToDocument(), new InsertOneOptions
        {
            Comment = "Insert from InsertAsync()"
        }, cancellationToken);
    }
}