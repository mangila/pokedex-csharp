using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using pokedex_shared.Model.Domain;
using pokedex_shared.Option;

namespace pokedex_shared.Service;

public class MongoDbGridFsService
{
    private readonly ILogger<MongoDbGridFsService> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly GridFSBucket _bucket;

    public MongoDbGridFsService(
        ILogger<MongoDbGridFsService> logger,
        IOptions<MongoDbOption> mongoDbOption,
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        var mongoDb = mongoDbOption.Value;
        var db = new MongoClient(mongoDb.ConnectionString)
            .GetDatabase(mongoDb.Database);
        _bucket = new GridFSBucket(db, new GridFSBucketOptions
        {
            BucketName = mongoDb.Bucket,
        });
    }

    public async Task<ObjectId> InsertAsync(Uri uri, PokemonName name)
    {
        var httpClient = _httpClientFactory.CreateClient();
        using var response = await httpClient.GetAsync(uri);
        response.EnsureSuccessStatusCode();
        await using var fileStream = await response.Content.ReadAsStreamAsync();
        return await _bucket.UploadFromStreamAsync($"{name.Value}-{Guid.NewGuid()}.png", fileStream,
            new GridFSUploadOptions
            {
                Metadata = new BsonDocument
                {
                    { "contentType", "image/png" },
                    { "description", "PNG image file from PokeAPI" },
                    { "uploadDate", BsonDateTime.Create(DateTime.UtcNow) }
                }
            });
    }

    public async Task<PokemonFileResult?> FindFileByIdAsync(ObjectId objectId,
        CancellationToken cancellationToken)
    {
        try
        {
            var cursor = await _bucket.FindAsync(Builders<GridFSFileInfo>.Filter.Eq("_id", objectId),
                cancellationToken: cancellationToken);
            var fileInfo = await cursor.FirstOrDefaultAsync(cancellationToken);
            var memoryStream = new MemoryStream();
            await _bucket.DownloadToStreamAsync(objectId, memoryStream, cancellationToken: cancellationToken);
            memoryStream.Position = 0;
            return new PokemonFileResult(fileInfo.Filename,
                fileInfo.Metadata["contentType"].AsString,
                memoryStream);
        }
        catch (Exception)
        {
            return null;
        }
    }
}