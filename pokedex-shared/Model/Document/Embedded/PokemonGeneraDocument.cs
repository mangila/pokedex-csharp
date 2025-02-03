using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;
using pokedex_shared.Model.Dto.Embedded;

namespace pokedex_shared.Model.Document.Embedded;

public readonly record struct PokemonGeneraDocument(
    [Required]
    [property: BsonElement("language")]
    string Language,
    [Required]
    [property: BsonElement("genera")]
    string Genera
);

public static partial class Extensions
{
    public static PokemonGeneraDto ToDto(this PokemonGeneraDocument document)
    {
        return new PokemonGeneraDto(
            document.Language,
            document.Genera
        );
    }

    public static ImmutableList<PokemonGeneraDto> ToDtos(this List<PokemonGeneraDocument> documents)
    {
        return documents
            .Select(document => document.ToDto())
            .ToImmutableList();
    }
}