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
public static class ApiMapper
{
    public static PokemonDocument ToDocument(PokemonApiResponse pokemonApiResponse,
        SpeciesApiResponse speciesApiResponse,
        EvolutionChainApiResponse evolutionChainApiResponse,
        ObjectId spriteId,
        ObjectId audioId)
    {
        return new PokemonDocument
        {
            PokemonId = pokemonApiResponse.id.ToString(),
            Name = pokemonApiResponse.name,
            Types = ToPokemonTypes(pokemonApiResponse.types),
            AudioId = audioId,
            SpriteId = spriteId,
        };
    }

    private static List<PokemonType> ToPokemonTypes(Types[] types)
    {
        var pokemonTypes = types.Select(t => new PokemonType
        {
            Type = t.type.name
        }).ToList();
        return [..pokemonTypes];
    }
}