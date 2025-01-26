using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;
using pokedex_shared.Model.Document.Embedded;
using pokedex_shared.Model.Dto;

namespace pokedex_shared.Model.Document;

public class PokemonDocument
{
    [Required] [BsonId] public required string PokemonId { get; init; }
    [Required] [BsonElement("name")] public required string Name { get; init; }
    [Required] [BsonElement("region")] public required string Region { get; init; }
    [Required] [BsonElement("height")] public required string Height { get; init; }
    [Required] [BsonElement("weight")] public required string Weight { get; init; }
    [Required] [BsonElement("description")] public required string Description { get; init; }
    [Required] [BsonElement("generation")] public required string Generation { get; init; }
    [Required] [BsonElement("types")] public required List<PokemonTypeDocument> Types { get; init; }
    [Required] [BsonElement("evolutions")] public required List<PokemonEvolutionDocument> Evolutions { get; init; }
    [Required] [BsonElement("stats")] public required List<PokemonStatDocument> Stats { get; init; }
    [Required] [BsonElement("images")] public required List<PokemonMediaDocument> Images { get; init; }
    [Required] [BsonElement("audios")] public required List<PokemonMediaDocument> Audios { get; init; }
    [Required] [BsonElement("legendary")] public required bool Legendary { get; init; }
    [Required] [BsonElement("mythical")] public required bool Mythical { get; init; }
    [Required] [BsonElement("baby")] public required bool Baby { get; init; }
}

public static class Extensions
{
    public static PokemonDto ToDto(this PokemonDocument document)
    {
        return new PokemonDto(
            PokemonId: document.PokemonId,
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