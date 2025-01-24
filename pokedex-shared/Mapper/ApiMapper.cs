using System.Globalization;
using System.Text.RegularExpressions;
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
        PokemonSpeciesApiResponse pokemonSpeciesApiResponse,
        EvolutionChainApiResponse evolutionChainApiResponse,
        List<PokemonMediaDocument> medias
    )
    {
        return new PokemonDocument
        {
            PokemonId = pokemonApiResponse.id.ToString(),
            Name = pokemonApiResponse.name,
            Height = ToMeterHeight(pokemonApiResponse.height),
            Weight = ToKilogramWeight(pokemonApiResponse.weight),
            Generation = pokemonSpeciesApiResponse.generation.name,
            Description = ToPokemonDescription(pokemonSpeciesApiResponse.flavor_text_entries),
            Stats = ToPokemonStats(pokemonApiResponse.stats),
            Medias = medias,
            Types = ToPokemonTypes(pokemonApiResponse.types),
            Evolutions = ToPokemonEvolutions(evolutionChainApiResponse.chain),
            Legendary = pokemonSpeciesApiResponse.is_legendary,
            Mythical = pokemonSpeciesApiResponse.is_mythical,
            Baby = pokemonSpeciesApiResponse.is_baby
        };
    }

    private static string ToMeterHeight(int height)
    {
        return (height / 10.0).ToString(CultureInfo.InvariantCulture);
    }

    private static string ToKilogramWeight(int weight)
    {
        return (weight / 10.0).ToString(CultureInfo.InvariantCulture);
    }

    private static List<PokemonEvolutionDocument> ToPokemonEvolutions(Chain chain)
    {
        if (chain.chain.Length == 0)
        {
            return [];
        }

        var list = new List<PokemonEvolutionDocument> { new(0, chain.species.name) };
        return GetEvolution(chain.chain, list);
    }

    private static List<PokemonEvolutionDocument> GetEvolution(EvolutionChain[] chainEvolvesTo,
        List<PokemonEvolutionDocument> list)
    {
        if (chainEvolvesTo.Length == 0) return list;

        foreach (var evolutionChain in chainEvolvesTo)
        {
            list.Add(new PokemonEvolutionDocument(list.Count, evolutionChain.species.name));
            GetEvolution(evolutionChain.next, list);
        }

        return list;
    }


    private static string ToPokemonDescription(Flavor_text_entries[] flavorTextEntries)
    {
        var flavorText = flavorTextEntries.First(entries => entries.language.name == "en").flavor_text;
        return ReplaceLineBreaks().Replace(flavorText, " ").Trim();
    }

    private static List<PokemonStatDocument> ToPokemonStats(Stats[] stats)
    {
        var total = 0;
        var l = stats.Select(s =>
            {
                total = total + s.base_stat ?? 0;
                return new PokemonStatDocument(s.stat.name, s.base_stat ?? 0);
            }
        ).ToList();
        l.Add(new PokemonStatDocument("total", total));
        return l;
    }

    private static List<PokemonTypeDocument> ToPokemonTypes(Types[] types)
    {
        return types.Select(t => new PokemonTypeDocument(t.type.name)).ToList();
    }

    [GeneratedRegex(@"[\r\n\t\f]+")]
    private static partial Regex ReplaceLineBreaks();
}