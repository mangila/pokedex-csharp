using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;
using pokedex_shared.Model.Dto.Embedded;

namespace pokedex_shared.Model.Document.Embedded;

public readonly record struct PokemonEvolutionDocument(
    [Required]
    [property: BsonElement("value")]
    int Value,
    [Required]
    [property: BsonElement("name")]
    string Name);

public static partial class Extensions
{
    public static ImmutableList<PokemonEvolutionDto> ToDtos(this List<PokemonEvolutionDocument> documents)
    {
        return documents
            .Select(document => document.ToDto())
            .ToImmutableList();
    }

    public static PokemonEvolutionDto ToDto(this PokemonEvolutionDocument document)
    {
        return new PokemonEvolutionDto(
            Value: document.Value,
            Name: document.Name
        );
    }
}