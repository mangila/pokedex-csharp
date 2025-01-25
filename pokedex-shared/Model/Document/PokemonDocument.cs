using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using pokedex_shared.Model.Dto;

namespace pokedex_shared.Model.Document;

public class PokemonDocument
{
    [Required] [BsonId] public required string PokemonId { get; init; }

    [Required] [BsonElement("name")] public required string Name { get; init; }
    [Required] [BsonElement("height")] public required string Height { get; init; }
    [Required] [BsonElement("weight")] public required string Weight { get; init; }

    [Required]
    [BsonElement("description")]
    public required string Description { get; init; }

    [Required] [BsonElement("generation")] public required string Generation { get; init; }
    [Required] [BsonElement("types")] public required List<PokemonTypeDocument> Types { get; init; }
    [Required] [BsonElement("evolutions")] public required List<PokemonEvolutionDocument> Evolutions { get; init; }
    [Required] [BsonElement("stats")] public required List<PokemonStatDocument> Stats { get; init; }
    [Required] [BsonElement("medias")] public required List<PokemonMediaDocument> Medias { get; init; }
    [Required] [BsonElement("legendary")] public required bool Legendary { get; init; }
    [Required] [BsonElement("mythical")] public required bool Mythical { get; init; }
    [Required] [BsonElement("baby")] public required bool Baby { get; init; }
}

public readonly record struct PokemonTypeDocument(
    [Required]
    [property: BsonElement("type")]
    string Type);

public readonly record struct PokemonStatDocument(
    [Required]
    [property: BsonElement("type")]
    string Type,
    [Required]
    [property: BsonElement("value")]
    int Value);

public readonly record struct PokemonEvolutionDocument(
    [Required]
    [property: BsonElement("value")]
    int Value,
    [Required]
    [property: BsonElement("name")]
    string Name);

public readonly record struct PokemonMediaDocument(
    [Required]
    [property: BsonElement("media_id")]
    string MediaId,
    [Required]
    [property: BsonElement("src")]
    string Src,
    [Required]
    [property: BsonElement("file_name")]
    string FileName,
    [Required]
    [property: BsonElement("content_type")]
    string ContentType);

public static class Extensions
{
    public static PokemonDto ToDto(this PokemonDocument document)
    {
        return new PokemonDto(
            PokemonId: document.PokemonId,
            Name: document.Name,
            Height: document.Height,
            Weight: document.Weight,
            Description: document.Description,
            Generation: document.Generation,
            Types: document.Types.ToDtos(),
            Evolutions: document.Evolutions.ToDtos(),
            Stats: document.Stats.ToDtos(),
            Medias: document.Medias.ToDtos(),
            Legendary: document.Legendary,
            Mythical: document.Mythical,
            Baby: document.Baby
        );
    }

    private static List<PokemonTypeDto> ToDtos(this List<PokemonTypeDocument> pokemonTypeDocuments)
    {
        return pokemonTypeDocuments.Select(document => document.ToDto()).ToList();
    }

    private static PokemonTypeDto ToDto(this PokemonTypeDocument pokemonTypeDocument)
    {
        return new PokemonTypeDto(pokemonTypeDocument.Type);
    }

    private static List<PokemonEvolutionDto> ToDtos(this List<PokemonEvolutionDocument> pokemonEvolutionDocuments)
    {
        return pokemonEvolutionDocuments.Select(document => document.ToDto()).ToList();
    }

    private static PokemonEvolutionDto ToDto(this PokemonEvolutionDocument pokemonEvolutionDocument)
    {
        return new PokemonEvolutionDto(
            Value: pokemonEvolutionDocument.Value,
            Name: pokemonEvolutionDocument.Name
        );
    }

    private static List<PokemonStatDto> ToDtos(this List<PokemonStatDocument> pokemonStatDocuments)
    {
        return pokemonStatDocuments.Select(document => document.ToDto()).ToList();
    }

    private static PokemonStatDto ToDto(this PokemonStatDocument pokemonStatDocument)
    {
        return new PokemonStatDto(
            Value: pokemonStatDocument.Value,
            Type: pokemonStatDocument.Type
        );
    }

    private static List<PokemonMediaDto> ToDtos(this List<PokemonMediaDocument> pokemonMediaDocuments)
    {
        return pokemonMediaDocuments.Select(document => document.ToDto()).ToList();
    }

    private static PokemonMediaDto ToDto(this PokemonMediaDocument pokemonMediaDocument)
    {
        return new PokemonMediaDto(
            MediaId: pokemonMediaDocument.MediaId,
            FileName: pokemonMediaDocument.FileName,
            ContentType: pokemonMediaDocument.ContentType,
            Src: pokemonMediaDocument.Src
        );
    }
}