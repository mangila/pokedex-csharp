using System.Globalization;
using System.Text.RegularExpressions;
using pokedex_shared.Http.EvolutionChain;
using pokedex_shared.Http.Pokemon;
using pokedex_shared.Http.Species;
using pokedex_shared.Model.Document;
using pokedex_shared.Model.Document.Embedded;
using pokedex_shared.Model.Domain;
using EvolutionChain = pokedex_shared.Http.EvolutionChain.EvolutionChain;

namespace pokedex_shared.Mapper;

/**
 * <summary>
 *  Map responses from PokeApi to database document
 * </summary>
 */
public static partial class ApiMapper
{
    public static PokemonDocument ToDocument(
        PokemonId pokemonId,
        PokemonName pokemonName,
        string generation,
        string region,
        List<PokemonMediaDocument> images,
        List<PokemonMediaDocument> audios,
        PokemonApiResponse[] varieties,
        PokemonApiResponse pokemonApiResponse,
        PokemonSpeciesApiResponse pokemonSpeciesApiResponse,
        EvolutionChainApiResponse evolutionChainApiResponse)
    {
        return new PokemonDocument
        {
            PokemonId = Convert.ToInt32(pokemonId.Value),
            Name = pokemonName.Value,
            JapaneseSignName = ToJapaneseSignName(pokemonSpeciesApiResponse.Names),
            Region = region,
            Height = ToMeterHeight(pokemonApiResponse.Height),
            Weight = ToKilogramWeight(pokemonApiResponse.Weight),
            Generation = generation,
            Description = ToPokemonDescription(pokemonSpeciesApiResponse.FlavorTextEntries),
            Stats = ToPokemonStats(pokemonApiResponse.Stats),
            Types = ToPokemonTypes(pokemonApiResponse.Types),
            Evolutions = ToPokemonEvolutions(evolutionChainApiResponse.Chain),
            Legendary = pokemonSpeciesApiResponse.Legendary,
            Mythical = pokemonSpeciesApiResponse.Mythical,
            Baby = pokemonSpeciesApiResponse.Baby,
            Images = images,
            Audios = audios,
            Varieties = ToVarieties(pokemonName, varieties)
        };
    }

    private static List<PokemonVarietyDocument> ToVarieties(PokemonName pokemonName, PokemonApiResponse[] varieties)
    {
        return varieties
            .Where(response => response.Name != pokemonName.Value)
            .Select(response => new PokemonVarietyDocument(response.Id, response.Name))
            .ToList();
    }

    private static string ToJapaneseSignName(Names[] names)
    {
        return names
            .First(name => name.Language.Name == "ja-Hrkt")
            .Name;
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
        if (chainEvolvesTo.Length == 0) return list;

        foreach (var evolutionChain in chainEvolvesTo)
        {
            list.Add(new PokemonEvolutionDocument(list.Count, evolutionChain.Species.Name));
            GetEvolution(evolutionChain.NextChain, list);
        }

        return list;
    }


    private static string ToPokemonDescription(FlavorTextEntries[] flavorTextEntries)
    {
        var flavorText = flavorTextEntries
            .First(entries => entries.Language.Name == "en")
            .FlavorText;
        return ReplaceLineBreaks()
            .Replace(flavorText, " ")
            .Trim();
    }

    private static List<PokemonStatDocument> ToPokemonStats(Stats[] stats)
    {
        var total = 0;
        var l = stats
            .Select(s =>
                {
                    total += s.BaseStat;
                    return new PokemonStatDocument(s.Stat.Name, s.BaseStat);
                }
            )
            .ToList();
        l.Add(new PokemonStatDocument("total", total));
        return l;
    }

    private static List<PokemonTypeDocument> ToPokemonTypes(Types[] types)
    {
        return types
            .Select(t => new PokemonTypeDocument(t.Type.Name))
            .ToList();
    }

    private static List<PokemonMediaDocument> ToPokemonAudios(List<PokemonMediaDocument> medias)
    {
        return medias
            .Where(document => document.ContentType.StartsWith("audio"))
            .ToList();
    }

    private static List<PokemonMediaDocument> ToPokemonImages(List<PokemonMediaDocument> medias)
    {
        return medias
            .Where(document => document.ContentType.StartsWith("image"))
            .ToList();
    }

    [GeneratedRegex(@"[\r\n\t\f]+")]
    private static partial Regex ReplaceLineBreaks();
}