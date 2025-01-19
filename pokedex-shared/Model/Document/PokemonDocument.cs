using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using pokedex_shared.Model.Dto;

namespace pokedex_shared.Model.Document;

public class PokemonDocument
{
    [Required] [BsonId] public ObjectId Id { get; init; }

    [Required]
    [StringLength(5, ErrorMessage = "length cannot be over 5")]
    [RegularExpression("\\d+", ErrorMessage = "must be a number")]
    [BsonElement("pokemon_id")]
    public required string PokemonId { get; init; }

    [Required]
    [StringLength(100, ErrorMessage = "Name cannot be more than 100 chars")]
    [BsonElement("name")]
    public required string Name { get; init; }
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