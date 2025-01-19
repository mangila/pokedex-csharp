using pokedex_shared.Http.EvolutionChain;
using pokedex_shared.Http.Pokemon;
using pokedex_shared.Http.Species;
using pokedex_shared.Model.Document;

namespace pokedex_shared.Mapper;

/**
 * <summary>
 *  Map from PokeApi api responses
 * </summary>
 */
public static class ApiMapper
{
    public static PokemonDocument ToDocument(
        PokemonApiResponse pokemonApiResponse,
        SpeciesApiResponse speciesApiResponse,
        EvolutionChainApiResponse evolutionChainApiResponse)
    {
        return new PokemonDocument
        {
            PokemonId = pokemonApiResponse.id.ToString(),
            Name = pokemonApiResponse.name
        };
    }
}