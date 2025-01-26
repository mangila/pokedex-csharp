using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;
using pokedex_shared.Model.Dto.Embedded;

namespace pokedex_shared.Model.Document.Embedded;

public readonly record struct PokemonMediaDocument(
    [Required]
    [property: BsonElement("media_id")]
    string MediaId,
    [Required]
    [property: BsonElement("src")]
    string Src,
    [Required]
    [property: BsonElement("file_name")]
    string FileName,
    [Required]
    [property: BsonElement("content_type")]
    string ContentType);

public static partial class Extensions
{
    public static List<PokemonMediaDto> ToDtos(this List<PokemonMediaDocument> documents)
    {
        return documents
            .Select(document => document.ToDto())
            .ToList();
    }

    public static PokemonMediaDto ToDto(this PokemonMediaDocument document)
    {
        return new PokemonMediaDto(
            MediaId: document.MediaId,
            Src: document.Src,
            FileName: document.FileName,
            ContentType: document.ContentType
        );
    }
}