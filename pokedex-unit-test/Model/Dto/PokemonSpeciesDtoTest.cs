using FluentAssertions;
using pokedex_shared.Common;
using pokedex_shared.Model.Dto;

namespace pokedex_unit_test.Model.Dto;

[TestFixture]
[TestOf(typeof(PokemonSpeciesDto))]
public class PokemonSpeciesDtoTest
{
    [Test]
    public async Task JsonTest()
    {
        var json = """
                   {
                     "id": 1,
                     "name": "bulbasaur",
                     "names": [
                       {
                         "language": "en",
                         "name": "Bulbasaur"
                       }
                     ],
                     "descriptions": [
                       {
                         "language": "en",
                         "description": "A strange seed was planted on its back at birth. The plant sprouts and grows with this POKéMON."
                       }
                     ],
                     "genera": [
                       {
                         "language": "en",
                         "genera": "Seed Pokémon"
                       },
                       {
                         "language": "ja",
                         "genera": "たねポケモン"
                       }
                     ],
                     "pedigree": {
                       "generation": "generation-i",
                       "region": "kanto"
                     },
                     "evolutions": [
                       {
                         "value": 0,
                         "name": "bulbasaur"
                       },
                       {
                         "value": 1,
                         "name": "ivysaur"
                       },
                       {
                         "value": 2,
                         "name": "venusaur"
                       }
                     ],
                     "varieties": [
                       {
                         "name": "bulbasaur",
                         "default": true,
                         "height": "0.7",
                         "weight": "6.9",
                         "types": [
                           {
                             "type": "grass"
                           },
                           {
                             "type": "poison"
                           }
                         ],
                         "stats": [
                           {
                             "type": "hp",
                             "value": 45
                           },
                           {
                             "type": "attack",
                             "value": 49
                           },
                           {
                             "type": "defense",
                             "value": 49
                           },
                           {
                             "type": "special-attack",
                             "value": 65
                           },
                           {
                             "type": "special-defense",
                             "value": 65
                           },
                           {
                             "type": "speed",
                             "value": 45
                           },
                           {
                             "type": "total",
                             "value": 318
                           }
                         ],
                         "images": [
                           {
                             "media_id": "67a8eb8eb37f566a15b30701",
                             "src": "http://localhost:5144/api/v1/pokemon/file/67a8eb8eb37f566a15b30701",
                             "file_name": "bulbasaur-front-default.webp",
                             "content_type": "image/webp"
                           },
                           {
                             "media_id": "67a8eb8eb37f566a15b30703",
                             "src": "http://localhost:5144/api/v1/pokemon/file/67a8eb8eb37f566a15b30703",
                             "file_name": "bulbasaur-back-default.webp",
                             "content_type": "image/webp"
                           }
                         ],
                         "audios": [
                           {
                             "media_id": "67a8eb8fb37f566a15b3075f",
                             "src": "http://localhost:5144/api/v1/pokemon/file/67a8eb8fb37f566a15b3075f",
                             "file_name": "bulbasaur-legacy.ogg",
                             "content_type": "audio/ogg"
                           },
                           {
                             "media_id": "67a8eb8fb37f566a15b30761",
                             "src": "http://localhost:5144/api/v1/pokemon/file/67a8eb8fb37f566a15b30761",
                             "file_name": "bulbasaur-latest.ogg",
                             "content_type": "audio/ogg"
                           }
                         ]
                       }
                     ],
                     "special": {
                       "special": false,
                       "legendary": false,
                       "mythical": false,
                       "baby": false
                     }
                   }
                   """;
        var dto = await json.DeserializeValueTypeToJsonAsync<PokemonSpeciesDto>(CancellationToken.None);
        dto.Should().NotBeNull();
        dto.Id.Should().Be(1);
        dto.Name.Should().Be("bulbasaur");
        dto.Names.Should()
            .HaveCount(1)
            .And
            .Contain(nameDto => nameDto.Name == "Bulbasaur" && nameDto.Language == "en");
        dto.Descriptions.Should()
            .HaveCount(1)
            .And
            .Contain(descriptionDto => descriptionDto.Language == "en" && descriptionDto.Description ==
                "A strange seed was planted on its back at birth. The plant sprouts and grows with this POKéMON.");
        dto.Genera.Should()
            .HaveCount(2)
            .And
            .Contain(generaDto => generaDto.Language == "en" && generaDto.Genera == "Seed Pokémon")
            .And
            .Contain(generaDto => generaDto.Language == "ja" && generaDto.Genera == "たねポケモン");
        dto.Pedigree.Region.Should().Be("kanto");
        dto.Pedigree.Generation.Should().Be("generation-i");
        dto.Evolutions.Should()
            .HaveCount(3)
            .And
            .Contain(evolutionDto => evolutionDto.Value == 0 && evolutionDto.Name == "bulbasaur")
            .And
            .Contain(evolutionDto => evolutionDto.Value == 1 && evolutionDto.Name == "ivysaur")
            .And
            .Contain(evolutionDto => evolutionDto.Value == 2 && evolutionDto.Name == "venusaur");
        dto.Varieties
            .Should()
            .HaveCount(1)
            .And
            .Contain(pokemonDto => pokemonDto.Name == "bulbasaur")
            .And
            .Contain(pokemonDto => pokemonDto.Default == true)
            .And
            .Contain(pokemonDto => pokemonDto.Height == "0.7")
            .And
            .Contain(pokemonDto => pokemonDto.Weight == "6.9")
            .And
            .Contain(pokemonDto => pokemonDto.Types.Count == 2)
            .And
            .Contain(pokemonDto => pokemonDto.Types.Any(typeDto => typeDto.Type == "grass"))
            .And
            .Contain(pokemonDto => pokemonDto.Types.Any(typeDto => typeDto.Type == "poison"))
            .And
            .Contain(pokemonDto => pokemonDto.Stats.Count == 7)
            .And
            .Contain(pokemonDto => pokemonDto.Stats.Any(statDto => statDto.Type == "hp" && statDto.Value == 45))
            .And
            .Contain(pokemonDto => pokemonDto.Stats.Any(statDto => statDto.Type == "attack" && statDto.Value == 49))
            .And
            .Contain(pokemonDto => pokemonDto.Stats.Any(statDto => statDto.Type == "defense" && statDto.Value == 49))
            .And
            .Contain(pokemonDto =>
                pokemonDto.Stats.Any(statDto => statDto.Type == "special-attack" && statDto.Value == 65))
            .And
            .Contain(pokemonDto =>
                pokemonDto.Stats.Any(statDto => statDto.Type == "special-defense" && statDto.Value == 65))
            .And
            .Contain(pokemonDto => pokemonDto.Stats.Any(statDto => statDto.Type == "speed" && statDto.Value == 45))
            .And
            .Contain(pokemonDto => pokemonDto.Stats.Any(statDto => statDto.Type == "total" && statDto.Value == 318))
            .And
            .Contain(pokemonDto => pokemonDto.Audios.Any(mediaDto => mediaDto.ContentType == "audio/ogg"))
            .And
            .Contain(pokemonDto => pokemonDto.Images.Any(mediaDto => mediaDto.ContentType == "image/webp"));
        dto.Special.Special.Should().Be(false);
        dto.Special.Baby.Should().Be(false);
        dto.Special.Legendary.Should().Be(false);
        dto.Special.Mythical.Should().Be(false);
        var serializedJson = await dto.ToJsonValueTypeAsync(CancellationToken.None);
        serializedJson
            .Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(json);
    }
}