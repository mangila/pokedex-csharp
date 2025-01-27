using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using pokedex_shared.Model.Document.Embedded;
using pokedex_shared.Model.Dto;
using pokedex_shared.Model.Dto.Collection;

namespace pokedex_shared.Model.Document.Projection;

public readonly record struct PokemonMediaProjection(
    [Required]
    [property: BsonId]
    [property: BsonRepresentation(BsonType.Int32)]
    int PokemonId,
    [Required]
    [property: BsonElement("name")]
    string Name,
    [Required]
    [property: BsonElement("images")]
    List<PokemonMediaDocument> Images
);

public static class Extensions
{
    public static PokemonMediaProjectionDtoCollection ToDtoCollection(
        this List<PokemonMediaProjection> documents)
    {
        var collection = documents
            .Select(doc => doc.ToDto())
            .ToImmutableList();
        return new PokemonMediaProjectionDtoCollection(collection);
    }

    public static PokemonMediaProjectionDto ToDto(
        this PokemonMediaProjection document)
    {
        return new PokemonMediaProjectionDto(
            PokemonId: document.PokemonId.ToString(),
            Name: document.Name,
            Images: document.Images.ToDtos()
        );
    }
}