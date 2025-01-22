using System.Text.RegularExpressions;
using MongoDB.Bson;
using pokedex_shared.Http.EvolutionChain;
using pokedex_shared.Http.Pokemon;
using pokedex_shared.Http.Species;
using pokedex_shared.Model.Document;

namespace pokedex_shared.Mapper;

/**
 * <summary>
 *  Map responses from PokeApi to database document
 * </summary>
 */
public static partial class ApiMapper
{
    public static PokemonDocument ToDocument(
        PokemonApiResponse pokemonApiResponse,
        SpeciesApiResponse speciesApiResponse,
        EvolutionChainApiResponse evolutionChainApiResponse,
        ObjectId spriteId,
        ObjectId audioId)
    {
        ToPokemonEvolutions(evolutionChainApiResponse.chain);

        return new PokemonDocument
        {
            PokemonId = pokemonApiResponse.id.ToString(),
            Name = pokemonApiResponse.name,
            Description = ToPokemonDescription(speciesApiResponse.flavor_text_entries),
            Stats = ToPokemonStats(pokemonApiResponse.stats),
            Types = ToPokemonTypes(pokemonApiResponse.types),
            Evolutions = ToPokemonEvolutions(evolutionChainApiResponse.chain),
            SpriteId = spriteId,
            AudioId = audioId,
        };
    }

    private static List<PokemonEvolution> ToPokemonEvolutions(Chain chain)
    {
        var list = new List<PokemonEvolution> { new(0, chain.species.name) };
        return GetEvolution(chain.evolves_to, list);
    }

    private static List<PokemonEvolution> GetEvolution(Evolves_to[] chainEvolvesTo, List<PokemonEvolution> list)
    {
        if (chainEvolvesTo.Length == 0) return list;

        foreach (var to in chainEvolvesTo)
        {
            list.Add(new PokemonEvolution(list.Count, to.species.name));
            GetEvolution(to.evolves_to, list);
        }

        return list;
    }


    private static string ToPokemonDescription(Flavor_text_entries[] flavorTextEntries)
    {
        var flavorText = flavorTextEntries.First(entries => entries.language.name == "en").flavor_text;
        return ReplaceLineBreaks().Replace(flavorText, " ").Trim();
    }

    private static List<PokemonStat> ToPokemonStats(Stats[] stats)
    {
        return stats.Select(s => new PokemonStat(s.stat.name, s.base_stat)).ToList();
    }

    private static List<PokemonType> ToPokemonTypes(Types[] types)
    {
        return types.Select(t => new PokemonType(t.type.name)).ToList();
    }

    [GeneratedRegex(@"[\r\n\t\f]+")]
    private static partial Regex ReplaceLineBreaks();
}