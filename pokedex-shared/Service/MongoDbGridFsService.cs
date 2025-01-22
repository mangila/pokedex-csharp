using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
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

    public async Task<ObjectId> InsertAsync(Uri uri, string name)
    {
        var httpClient = _httpClientFactory.CreateClient();
        using var response = await httpClient.GetAsync(uri);
        response.EnsureSuccessStatusCode();
        await using var fileStream = await response.Content.ReadAsStreamAsync();
        var objectId = await _bucket.UploadFromStreamAsync(name, fileStream);
        return objectId;
    }
}