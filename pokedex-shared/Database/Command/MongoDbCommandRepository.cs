using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using pokedex_shared.Common.Option;
using pokedex_shared.Model.Document;

namespace pokedex_shared.Database.Command;

public class MongoDbCommandRepository
{
    private readonly ILogger<MongoDbCommandRepository> _logger;
    private readonly IMongoCollection<PokemonSpeciesDocument> _collection;

    public MongoDbCommandRepository(
        ILogger<MongoDbCommandRepository> logger,
        IOptions<MongoDbOption> mongoDbOption)
    {
        _logger = logger;
        var mongoDb = mongoDbOption.Value;
        _collection = new MongoClient(mongoDb.ConnectionString)
            .GetDatabase(mongoDb.Database)
            .GetCollection<PokemonSpeciesDocument>(mongoDb.Collection);
    }


    public async Task ReplaceOneAsync(
        PokemonSpeciesDocument document,
        CancellationToken cancellationToken = default)
    {
        await _collection.ReplaceOneAsync(
            doc => doc.Id == document.Id,
            document,
            new ReplaceOptions
            {
                IsUpsert = true,
                Comment = "Insert from ReplaceOneAsync() - replace or insert a new Pokemon"
            },
            cancellationToken: cancellationToken);
    }
}