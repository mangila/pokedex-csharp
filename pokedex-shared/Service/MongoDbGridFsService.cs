using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using pokedex_shared.Model.Document;
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

    public async Task<PokemonMediaDocument> InsertAsync(
        Uri uri,
        string fileName,
        string contentType,
        string description,
        CancellationToken cancellationToken = default)
    {
        var filter = Builders<GridFSFileInfo>.Filter.Eq(file => file.Filename, fileName);
        var cursor = await _bucket.FindAsync(filter, cancellationToken: cancellationToken);
        var fileInfo = await cursor.FirstOrDefaultAsync(cancellationToken);
        if (fileInfo is not null)
        {
            _logger.LogInformation("{fileName} is already uploaded", fileName);
            return new PokemonMediaDocument(
                MediaId: fileInfo.Id.ToString(),
                FileName: fileInfo.Filename,
                ContentType: fileInfo.Metadata["content_type"].AsString
            );
        }

        _logger.LogInformation("{fileName} upload", fileName);
        var httpClient = _httpClientFactory.CreateClient();
        using var response = await httpClient.GetAsync(uri, cancellationToken);
        response.EnsureSuccessStatusCode();
        await using var fileStream = await response.Content.ReadAsStreamAsync(cancellationToken);
        var mediaId = await _bucket.UploadFromStreamAsync(fileName, fileStream,
            new GridFSUploadOptions
            {
                Metadata = new BsonDocument
                {
                    { "content_type", contentType },
                    { "description", description },
                    { "upload_date", BsonDateTime.Create(DateTime.UtcNow) }
                }
            }, cancellationToken);
        return new PokemonMediaDocument(
            MediaId: mediaId.ToString(),
            FileName: fileName,
            ContentType: contentType
        );
    }

    public async Task<PokemonFileResult?> FindFileByIdAsync(ObjectId objectId,
        CancellationToken cancellationToken)
    {
        var cursor = await _bucket.FindAsync(Builders<GridFSFileInfo>.Filter.Eq("_id", objectId),
            cancellationToken: cancellationToken);
        var fileInfo = await cursor.FirstOrDefaultAsync(cancellationToken);
        if (fileInfo is null)
        {
            _logger.LogInformation("{objectId} not found", objectId.ToString());
            return null;
        }

        var memoryStream = new MemoryStream();
        await _bucket.DownloadToStreamAsync(objectId, memoryStream, cancellationToken: cancellationToken);
        memoryStream.Position = 0;
        return new PokemonFileResult(fileInfo.Filename,
            fileInfo.Metadata["contentType"].AsString,
            memoryStream);
    }
}