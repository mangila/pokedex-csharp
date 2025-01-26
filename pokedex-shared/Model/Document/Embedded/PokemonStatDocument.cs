using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;
using pokedex_shared.Model.Dto.Embedded;

namespace pokedex_shared.Model.Document.Embedded;

public readonly record struct PokemonStatDocument(
    [Required]
    [property: BsonElement("type")]
    string Type,
    [Required]
    [property: BsonElement("value")]
    int Value);

public static partial class Extensions
{
    public static ImmutableList<PokemonStatDto> ToDtos(this List<PokemonStatDocument> documents)
    {
        return documents
            .Select(document => document.ToDto())
            .ToImmutableList();
    }

    public static PokemonStatDto ToDto(this PokemonStatDocument document)
    {
        return new PokemonStatDto(
            Value: document.Value,
            Type: document.Type
        );
    }
}