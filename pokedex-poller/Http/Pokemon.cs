using System.Text.Json.Serialization;

namespace pokedex_poller.Http;

public class Pokemon
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; }
}