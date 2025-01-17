using System.Text.Json.Serialization;

namespace pokedex_shared.Model;

public class Pokemon
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; }
}