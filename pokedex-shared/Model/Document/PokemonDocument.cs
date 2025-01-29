using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;
using pokedex_shared.Http.Species;
using pokedex_shared.Model.Document.Embedded;
using pokedex_shared.Model.Dto;

namespace pokedex_shared.Model.Document;

public readonly record struct PokemonDocument(
    [Required] [property: BsonId] int PokemonId,
    [Required]
    [property: BsonElement("name")]
    string Name,
    [Required]
    [property: BsonElement("region")]
    string Region,
    [Required]
    [property: BsonElement("height")]
    string Height,
    [Required]
    [property: BsonElement("weight")]
    string Weight,
    [Required]
    [property: BsonElement("description")]
    string Description,
    [Required]
    [property: BsonElement("generation")]
    string Generation,
    [Required]
    [property: BsonElement("types")]
    List<PokemonTypeDocument> Types,
    [Required]
    [property: BsonElement("evolutions")]
    List<PokemonEvolutionDocument> Evolutions,
    [Required]
    [property: BsonElement("stats")]
    List<PokemonStatDocument> Stats,
    [Required]
    [property: BsonElement("images")]
    List<PokemonMediaDocument> Images,
    [Required]
    [property: BsonElement("audios")]
    List<PokemonMediaDocument> Audios,
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
    public static ImmutableList<PokemonDto> ToDtos(this List<PokemonDocument> documents)
    {
        return documents
            .Select(document => document.ToDto())
            .ToImmutableList();
    }

    public static PokemonDto ToDto(this PokemonDocument document)
    {
        return new PokemonDto(
            PokemonId: document.PokemonId.ToString(),
            Name: document.Name,
            Region: document.Region,
            Height: document.Height,
            Weight: document.Weight,
            Description: document.Description,
            Generation: document.Generation,
            Types: document.Types.ToDtos(),
            Evolutions: document.Evolutions.ToDtos(),
            Stats: document.Stats.ToDtos(),
            Audios: document.Audios.ToDtos(),
            Images: document.Images.ToDtos(),
            Legendary: document.Legendary,
            Mythical: document.Mythical,
            Baby: document.Baby
        );
    }
}