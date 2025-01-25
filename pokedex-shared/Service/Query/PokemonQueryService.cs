using MongoDB.Bson;
using pokedex_shared.Model.Domain;
using pokedex_shared.Model.Dto;

namespace pokedex_shared.Service.Query;

public class PokemonQueryService(DatasourceQueryService datasourceQuery)
{
    public async Task<PokemonDtoCollection> FindAllByPokemonIdAsync(PokemonIdCollection pokemonIdCollection,
        CancellationToken cancellationToken = default)
    {
        return await datasourceQuery.FindAllByPokemonIdAsync(pokemonIdCollection, cancellationToken);
    }

    public async Task<PokemonDtoCollection> FindAllAsync(CancellationToken cancellationToken = default)
    {
        return await datasourceQuery.FindAllAsync(cancellationToken);
    }

    public async Task<PokemonDtoCollection> SearchByNameAsync(PokemonName search,
        CancellationToken cancellationToken = default)
    {
        return await datasourceQuery.SearchByNameAsync(search, cancellationToken);
    }

    public async Task<PokemonDto?> FindOneByPokemonIdAsync(PokemonId pokemonId,
        CancellationToken cancellationToken = default)
    {
        return await datasourceQuery.FindByPokemonIdAsync(pokemonId, cancellationToken);
    }

    public async Task<PokemonDto?> FindOneByNameAsync(PokemonName pokemonName,
        CancellationToken cancellationToken = default)
    {
        return await datasourceQuery.FindByNameAsync(pokemonName, cancellationToken);
    }

    public async Task<PokemonFileResult?> FindFileByIdAsync(ObjectId id,
        CancellationToken cancellationToken = default)
    {
        return await datasourceQuery.FindFileByIdAsync(id, cancellationToken);
    }
}