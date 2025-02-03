using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using pokedex_shared.Model.Document;
using pokedex_shared.Option;

namespace pokedex_shared.Service.Command;

public class MongoDbCommandService
{
    private readonly ILogger<MongoDbCommandService> _logger;
    private readonly IMongoCollection<PokemonDocument> _collection;

    public MongoDbCommandService(
        ILogger<MongoDbCommandService> logger,
        IOptions<MongoDbOption> mongoDbOption)
    {
        _logger = logger;
        var mongoDb = mongoDbOption.Value;
        _collection = new MongoClient(mongoDb.ConnectionString)
            .GetDatabase(mongoDb.Database)
            .GetCollection<PokemonDocument>(mongoDb.Collection);
    }


    public async Task ReplaceOneAsync(PokemonDocument document, CancellationToken cancellationToken = default)
    {
        await _collection.ReplaceOneAsync(
            doc => doc.PokemonId == document.PokemonId,
            document,
            new ReplaceOptions
            {
                IsUpsert = true,
                Comment = "Insert from ReplaceOneAsync() - replace or insert a new Pokemon"
            },
            cancellationToken: cancellationToken);
    }

    public async Task<PokemonDocument> GetByIdAsync(PokemonDocument pokemon,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}