using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;
using pokedex_shared.Model.Document.Embedded;
using pokedex_shared.Model.Dto;
using pokedex_shared.Model.Dto.Collection;

namespace pokedex_shared.Model.Document.Projection;

public readonly record struct PokemonMediaProjectionDocument(
    [Required] [property: BsonId] string PokemonId,
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
        this List<PokemonMediaProjectionDocument> documents)
    {
        var collection = documents
            .Select(doc => doc.ToDto())
            .ToImmutableList();
        return new PokemonMediaProjectionDtoCollection(collection);
    }

    public static PokemonMediaProjectionDto ToDto(
        this PokemonMediaProjectionDocument document)
    {
        return new PokemonMediaProjectionDto(
            PokemonId: document.PokemonId,
            Name: document.Name,
            Images: document.Images.ToDtos()
        );
    }
}