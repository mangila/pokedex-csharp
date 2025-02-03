using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;
using pokedex_shared.Model.Dto.Embedded;

namespace pokedex_shared.Model.Document.Embedded;

public readonly record struct PokemonDescriptionDocument(
    [Required]
    [property: BsonElement("language")]
    string Language,
    [Required]
    [property: BsonElement("description")]
    string Description
);

public static partial class Extensions
{
    public static PokemonDescriptionDto ToDto(this PokemonDescriptionDocument document)
    {
        return new PokemonDescriptionDto(
            document.Language,
            document.Description
        );
    }

    public static ImmutableList<PokemonDescriptionDto> ToDtos(this List<PokemonDescriptionDocument> documents)
    {
        return documents
            .Select(document => document.ToDto())
            .ToImmutableList();
    }
}