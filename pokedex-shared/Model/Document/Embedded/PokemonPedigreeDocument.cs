using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;
using pokedex_shared.Model.Dto.Embedded;

namespace pokedex_shared.Model.Document.Embedded;

public readonly record struct PokemonPedigreeDocument(
    [Required]
    [property: BsonElement("generation")]
    string Generation,
    [Required]
    [property: BsonElement("region")]
    string Region
);

public static partial class Extensions
{
    public static PokemonPedigreeDto ToDto(this PokemonPedigreeDocument document)
    {
        return new PokemonPedigreeDto(
            document.Generation,
            document.Region
        );
    }
}
