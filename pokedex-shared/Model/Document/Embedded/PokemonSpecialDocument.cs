using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;
using pokedex_shared.Model.Dto.Embedded;

namespace pokedex_shared.Model.Document.Embedded;

public readonly record struct PokemonSpecialDocument(
    [Required]
    [property: BsonElement("special")]
    bool Special,
    [Required]
    [property: BsonElement("legendary")]
    bool Legendary,
    [Required]
    [property: BsonElement("mythical")]
    bool Mythical,
    [Required]
    [property: BsonElement("baby")]
    bool Baby
);

public static partial class Extensions
{
    public static PokemonSpecialDto ToDto(this PokemonSpecialDocument document)
    {
        return new PokemonSpecialDto(
            document.Special,
            document.Legendary,
            document.Mythical,
            document.Baby
        );
    }
}