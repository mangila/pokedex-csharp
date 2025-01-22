using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using pokedex_shared.Model.Dto;

namespace pokedex_shared.Model.Document;

public class PokemonDocument
{
    [Required] [BsonId] public ObjectId Id { get; init; }
    [Required] [BsonElement("pokemon_id")] public required string PokemonId { get; init; }
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
    [Required] [BsonElement("audio_id")] public required ObjectId AudioId { get; init; }
    [Required] [BsonElement("sprite_id")] public required ObjectId SpriteId { get; init; }
    [Required] [BsonElement("legendary")] public required bool Legendary { get; init; }
    [Required] [BsonElement("mythical")] public required bool Mythical { get; init; }
    [Required] [BsonElement("baby")] public required bool Baby { get; init; }
}

public readonly record struct PokemonTypeDocument(string type);

public readonly record struct PokemonStatDocument(string type, int value);

public readonly record struct PokemonEvolutionDocument(int value, string name);

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
            AudioId: document.AudioId.ToString(),
            SpriteId: document.SpriteId.ToString(),
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
        return new PokemonTypeDto(pokemonTypeDocument.type);
    }

    private static List<PokemonEvolutionDto> ToDtos(this List<PokemonEvolutionDocument> pokemonEvolutionDocuments)
    {
        return pokemonEvolutionDocuments.Select(document => document.ToDto()).ToList();
    }

    private static PokemonEvolutionDto ToDto(this PokemonEvolutionDocument pokemonEvolutionDocument)
    {
        return new PokemonEvolutionDto(
            value: pokemonEvolutionDocument.value,
            name: pokemonEvolutionDocument.name
        );
    }

    private static List<PokemonStatDto> ToDtos(this List<PokemonStatDocument> pokemonStatDocuments)
    {
        return pokemonStatDocuments.Select(document => document.ToDto()).ToList();
    }

    private static PokemonStatDto ToDto(this PokemonStatDocument pokemonStatDocument)
    {
        return new PokemonStatDto(
            value: pokemonStatDocument.value,
            type: pokemonStatDocument.type
        );
    }
}