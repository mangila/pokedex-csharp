using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;
using pokedex_shared.Model.Dto.Embedded;

namespace pokedex_shared.Model.Document.Embedded;

public readonly record struct PokemonNameDocument(
    [Required]
    [property: BsonElement("language")]
    string Language,
    [Required]
    [property: BsonElement("name")]
    string Name
);

public static partial class Extensions
{
    public static PokemonNameDto ToDto(this PokemonNameDocument document)
    {
        return new PokemonNameDto(
            document.Language,
            document.Name
        );
    }

    public static ImmutableList<PokemonNameDto> ToDtos(this List<PokemonNameDocument> documents)
    {
        return documents
            .Select(document => document.ToDto())
            .ToImmutableList();
    }
}