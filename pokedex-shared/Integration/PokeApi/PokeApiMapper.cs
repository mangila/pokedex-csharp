using System.Globalization;
using System.Text.RegularExpressions;
using pokedex_shared.Integration.PokeApi.Response.EvolutionChain;
using pokedex_shared.Integration.PokeApi.Response.Pokemon;
using pokedex_shared.Integration.PokeApi.Response.Species;
using pokedex_shared.Model.Document;
using pokedex_shared.Model.Document.Embedded;
using pokedex_shared.Model.Domain;
using EvolutionChain = pokedex_shared.Integration.PokeApi.Response.EvolutionChain.EvolutionChain;

namespace pokedex_shared.Integration.PokeApi;

public static partial class PokeApiMapper
{
    [GeneratedRegex(@"[\r\n\t\f]+", RegexOptions.Compiled)]
    private static partial Regex ReplaceLineBreaks();

    public static PokemonDocument ToPokemonDocument(
        PokemonName name,
        bool isDefault,
        int weight,
        int height,
        Types[] types,
        Stats[] stats,
        List<PokemonMediaDocument> images,
        List<PokemonMediaDocument> audios)
    {
        return new PokemonDocument(
            Name: name.Value,
            Default: isDefault,
            Weight: ToKilogram(weight),
            Height: ToMeter(height),
            Types: ToTypes(types),
            Stats: ToStats(stats),
            Images: images,
            Audios: audios
        );
    }

    /**
     * <summary>
     * PokeApi returns height in Decimeters
     * </summary>
     */
    private static string ToMeter(int height)
    {
        return (height / 10.0).ToString(CultureInfo.InvariantCulture);
    }

    /**
     * <summary>
     *  PokeApi returns weight in Hectograms
     * </summary>
     */
    private static string ToKilogram(int weight)
    {
        return (weight * 0.1).ToString(CultureInfo.InvariantCulture);
    }

    private static List<PokemonTypeDocument> ToTypes(Types[] types)
    {
        return types
            .Select(t => PokemonType.From(t.Type.Name))
            .Select(t => new PokemonTypeDocument(t.Value))
            .ToList();
    }

    private static List<PokemonStatDocument> ToStats(Stats[] stats)
    {
        var total = 0;
        var statDocuments = stats.Select(stat =>
            {
                var pokemonStat = PokemonStat.From(stat.Stat.Name);
                pokemonStat.Value = stat.BaseStat;
                total += stat.BaseStat; // Accumulate the total
                return pokemonStat;
            })
            .Select(result => new PokemonStatDocument(result.Name, result.Value))
            .ToList();
        statDocuments.Add(new PokemonStatDocument("total", total));
        return statDocuments;
    }


    public static PokemonSpeciesDocument ToSpeciesDocument(
        PokemonId id,
        PokemonName name,
        PokemonGeneration generation,
        string region,
        EvolutionChainResponse evolutionChain,
        PokemonSpeciesResponse species,
        List<PokemonDocument> pokemons)
    {
        return new PokemonSpeciesDocument(
            Id: id.ToInt(),
            Name: name.Value,
            Names: ToNames(species.Names),
            Descriptions: ToDescriptions(species.FlavorTextEntries),
            Genera: ToGenera(species.Genera),
            Pedigree: new PokemonPedigreeDocument(generation.Value, region),
            Evolutions: ToEvolutions(evolutionChain.Chain),
            Varieties: pokemons,
            Special: ToSpecial(species.Legendary, species.Mythical, species.Baby)
        );
    }

    private static PokemonSpecialDocument ToSpecial(
        bool legendary,
        bool mythical,
        bool baby)
    {
        var special = legendary || mythical || baby;
        return new PokemonSpecialDocument(special, legendary, mythical, baby);
    }

    private static List<PokemonNameDocument> ToNames(Names[] names)
    {
        return names
            .Select(n => new PokemonNameDocument(
                n.Language.Name,
                n.Name))
            .ToList();
    }

    /**
     * <summary>
     *  Running Regex since flavor text returns with line breaks in the string
     * </summary>
     */
    private static List<PokemonDescriptionDocument> ToDescriptions(
        FlavorTextEntries[] flavorTextEntries)
    {
        return flavorTextEntries
            .Select(entries => new PokemonDescriptionDocument(
                entries.Language.Name,
                ReplaceLineBreaks()
                    .Replace(entries.FlavorText, " ")
                    .Trim()))
            .ToList();
    }

    private static List<PokemonGeneraDocument> ToGenera(Genera[] genera)
    {
        return genera
            .Select(g => new PokemonGeneraDocument(
                g.Language.Name,
                g.Genus))
            .ToList();
    }

    /**
     * <summary>
     *  Recursive method running to get the full depth of the Chain arrays
     * </summary>
     */
    private static List<PokemonEvolutionDocument> ToEvolutions(Chain chain)
    {
        if (chain.FirstChain.Length == 0)
        {
            return [];
        }

        var list = new List<PokemonEvolutionDocument> { new(0, chain.Species.Name) };
        return GetEvolution(chain.FirstChain, list);
    }

    private static List<PokemonEvolutionDocument> GetEvolution(EvolutionChain[] chainEvolvesTo,
        List<PokemonEvolutionDocument> list)
    {
        while (true)
        {
            if (chainEvolvesTo.Length == 0) return list;

            var chain = chainEvolvesTo[0];
            list.Add(new PokemonEvolutionDocument(list.Count, chain.Species.Name));
            chainEvolvesTo = chain.NextChain;
        }
    }
}