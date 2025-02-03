using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;
using pokedex_shared.Model.Dto.Embedded;

namespace pokedex_shared.Model.Document.Embedded;

public readonly record struct PokemonDocument(
    [Required]
    [property: BsonElement("name")]
    string Name,
    [Required]
    [property: BsonElement("default")]
    bool Default,
    [Required]
    [property: BsonElement("height")]
    string Height,
    [Required]
    [property: BsonElement("weight")]
    string Weight,
    [Required]
    [property: BsonElement("types")]
    List<PokemonTypeDocument> Types,
    [Required]
    [property: BsonElement("stats")]
    List<PokemonStatDocument> Stats,
    [Required]
    [property: BsonElement("images")]
    List<PokemonMediaDocument> Images,
    [Required]
    [property: BsonElement("audios")]
    List<PokemonMediaDocument> Audios
);

public static partial class Extensions
{
    public static ImmutableList<PokemonDto> ToDtos(this List<PokemonDocument> documents)
    {
        return documents
            .Select(document => document.ToDto())
            .ToImmutableList();
    }

    public static PokemonDto ToDto(this PokemonDocument document)
    {
        return new PokemonDto(
            Name: document.Name,
            Default: document.Default,
            Height: document.Height,
            Weight: document.Weight,
            Types: document.Types.ToDtos(),
            Stats: document.Stats.ToDtos(),
            Audios: document.Audios.ToDtos(),
            Images: document.Images.ToDtos()
        );
    }
}