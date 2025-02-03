using System.Globalization;
using pokedex_shared.Integration.PokeApi.Response.Pokemon;
using pokedex_shared.Model.Document;
using pokedex_shared.Model.Document.Embedded;
using pokedex_shared.Model.Domain;

namespace pokedex_shared.Integration.PokeApi;

public class PokeApiMapper
{
    public static PokemonSpeciesDocument ToSpeciesDocument()
    {
        throw new NotImplementedException();
    }

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
            Types: types,
            Stats: stats,
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
}