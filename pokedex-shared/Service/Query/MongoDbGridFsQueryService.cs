using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using pokedex_shared.Model.Domain;
using pokedex_shared.Option;

namespace pokedex_shared.Service.Query;

public class MongoDbGridFsQueryService
{
    private readonly ILogger<MongoDbGridFsQueryService> _logger;
    private readonly GridFSBucket _bucket;

    public MongoDbGridFsQueryService(
        ILogger<MongoDbGridFsQueryService> logger,
        IOptions<MongoDbOption> mongoDbOption)
    {
        _logger = logger;
        var mongoDb = mongoDbOption.Value;
        var db = new MongoClient(mongoDb.ConnectionString)
            .GetDatabase(mongoDb.Database);
        _bucket = new GridFSBucket(db, new GridFSBucketOptions
        {
            BucketName = mongoDb.Bucket,
        });
    }

    public async Task<PokemonFileResult?> FindFileByIdAsync(ObjectId objectId,
        CancellationToken cancellationToken)
    {
        var fileInfo = await (await _bucket
                .FindAsync(Builders<GridFSFileInfo>.Filter.Eq("_id", objectId), cancellationToken: cancellationToken))
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (fileInfo is null)
        {
            _logger.LogInformation("{objectId} not found", objectId.ToString());
            return null;
        }

        var memoryStream = new MemoryStream();
        await _bucket.DownloadToStreamAsync(objectId, memoryStream, cancellationToken: cancellationToken);
        memoryStream.Position = 0;
        return new PokemonFileResult(fileInfo.Filename,
            fileInfo.Metadata["content_type"].AsString,
            memoryStream);
    }
}