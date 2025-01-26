using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;
using pokedex_shared.Model.Document.Embedded;
using pokedex_shared.Model.Dto.Embedded;

namespace pokedex_shared.Model.Document.Projection;

public readonly record struct PokemonMediaProjection(
    [Required] [property: BsonId] string PokemonId,
    [Required]
    [property: BsonElement("name")]
    string Name,
    [Required]
    [property: BsonElement("images")]
    List<PokemonMediaDocument> Images
);

public static partial class Extensions
{
    public static List<PokemonTypeDto> ToDtos(this List<PokemonTypeDocument> documents)
    {
        return documents
            .Select(document => document.ToDto())
            .ToList();
    }

    public static PokemonTypeDto ToDto(this PokemonTypeDocument document)
    {
        return new PokemonTypeDto(document.Type);
    }
}