using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using pokedex_shared.Common.Option;
using pokedex_shared.Model.Document.Embedded;
using pokedex_shared.Model.Domain;
using static MongoDB.Driver.Builders<MongoDB.Driver.GridFS.GridFSFileInfo>;

namespace pokedex_shared.Database.Command;

public class MongoDbGridFsCommandRepository
{
    private readonly ILogger<MongoDbGridFsCommandRepository> _logger;
    private readonly GridFSBucket _bucket;
    private readonly PokedexApiOption _pokedexApiOption;

    public MongoDbGridFsCommandRepository(
        ILogger<MongoDbGridFsCommandRepository> logger,
        IOptions<MongoDbOption> mongoDbOption,
        IOptions<PokedexApiOption> pokedexApiOption)
    {
        _logger = logger;
        _pokedexApiOption = pokedexApiOption.Value;
        var mongoDb = mongoDbOption.Value;
        var db = new MongoClient(mongoDb.ConnectionString)
            .GetDatabase(mongoDb.Database);
        _bucket = new GridFSBucket(db, new GridFSBucketOptions
        {
            BucketName = mongoDb.Bucket,
        });
    }

    public async Task<PokemonMediaDocument> InsertAsync(
        PokemonMediaEntry entry,
        CancellationToken cancellationToken = default)
    {
        var filter = Filter
            .Eq(gridFsFileInfo => gridFsFileInfo.Filename, entry.FileName);
        var gridFsFileInfo = await _bucket
            .Find(filter, cancellationToken: cancellationToken)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (gridFsFileInfo is not null)
        {
            return new PokemonMediaDocument(
                MediaId: gridFsFileInfo.Id.ToString(),
                FileName: gridFsFileInfo.Filename,
                ContentType: gridFsFileInfo.Metadata["content_type"].AsString,
                Src: GetSrc(gridFsFileInfo.Id.ToString())
            );
        }

        var mediaId = await _bucket.UploadFromBytesAsync(entry.FileName, entry.File,
            new GridFSUploadOptions
            {
                Metadata = new BsonDocument
                {
                    { "content_type", entry.ContentType },
                }
            }, cancellationToken);
        return new PokemonMediaDocument(
            MediaId: mediaId.ToString(),
            FileName: entry.FileName,
            ContentType: entry.ContentType,
            Src: GetSrc(mediaId.ToString())
        );
    }

    private string GetSrc(string mediaId)
    {
        return $"{_pokedexApiOption.Url}/{GetFileUri(mediaId)}";
    }

    private string GetFileUri(string mediaId)
    {
        return _pokedexApiOption.GetFileUri.Replace("{id}", mediaId);
    }
}