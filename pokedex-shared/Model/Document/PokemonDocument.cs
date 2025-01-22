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
    [Required] [BsonElement("types")] public required List<PokemonType> Types { get; init; }
    [Required] [BsonElement("audio_id")] public required ObjectId AudioId { get; init; }
    [Required] [BsonElement("sprite_id")] public required ObjectId SpriteId { get; init; }
}

public static class Extensions
{
    public static PokemonDto ToDto(this PokemonDocument document)
    {
        return new PokemonDto(
            PokemonId: document.PokemonId,
            Name: document.Name
        );
    }
}