using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;
using pokedex_shared.Model.Document.Embedded;
using pokedex_shared.Model.Dto;

namespace pokedex_shared.Model.Document;

public readonly record struct PokemonSpeciesDocument(
    [Required] [property: BsonId] int Id,
    [Required]
    [property: BsonElement("name")]
    string Name,
    [Required]
    [property: BsonElement("names")]
    List<PokemonNameDocument> Names,
    [Required]
    [property: BsonElement("descriptions")]
    List<PokemonDescriptionDocument> Descriptions,
    [Required]
    [property: BsonElement("genera")]
    List<PokemonGeneraDocument> Genera,
    [Required]
    [property: BsonElement("pedigree")]
    PokemonPedigreeDocument Pedigree,
    [Required]
    [property: BsonElement("evolutions")]
    List<PokemonEvolutionDocument> Evolutions,
    [Required]
    [property: BsonElement("varieties")]
    List<PokemonDocument> Varieties,
    [Required]
    [property: BsonElement("special")]
    PokemonSpecialDocument Special
);

public static partial class Extensions
{
    public static ImmutableList<PokemonSpeciesDto> ToDtos(this List<PokemonSpeciesDocument> documents)
    {
        return documents
            .Select(document => document.ToDto())
            .ToImmutableList();
    }

    public static PokemonSpeciesDto ToDto(this PokemonSpeciesDocument document)
    {
        return new PokemonSpeciesDto(
            Id: document.Id,
            Name: document.Name,
            Names: document.Names.ToDtos(),
            Descriptions: document.Descriptions.ToDtos(),
            Genera: document.Genera.ToDtos(),
            Pedigree: document.Pedigree.ToDto(),
            Evolutions: document.Evolutions.ToDtos(),
            Varieties: document.Varieties.ToDtos(),
            Special: document.Special.ToDto()
        );
    }
}