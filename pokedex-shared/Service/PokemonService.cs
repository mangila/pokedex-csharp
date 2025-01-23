using MongoDB.Bson;
using MongoDB.Driver.GridFS;
using pokedex_shared.Model.Domain;
using pokedex_shared.Model.Dto;

namespace pokedex_shared.Service;

public class PokemonService(DatasourceService datasource)
{
    public async Task<PokemonDtoCollection> FindAllByPokemonIdAsync(PokemonIdCollection pokemonIdCollection,
        CancellationToken cancellationToken = default)
    {
        return await datasource.FindAllByPokemonIdAsync(pokemonIdCollection, cancellationToken);
    }

    public async Task<PokemonDtoCollection> FindAllAsync(CancellationToken cancellationToken = default)
    {
        return await datasource.FindAllAsync(cancellationToken);
    }

    public async Task<PokemonDtoCollection> SearchByNameAsync(PokemonName search,
        CancellationToken cancellationToken = default)
    {
        return await datasource.SearchByNameAsync(search, cancellationToken);
    }

    public async Task<PokemonDto?> FindOneByPokemonIdAsync(PokemonId pokemonId,
        CancellationToken cancellationToken = default)
    {
        return await datasource.FindByPokemonIdAsync(pokemonId, cancellationToken);
    }

    public async Task<PokemonDto?> FindOneByNameAsync(PokemonName pokemonName,
        CancellationToken cancellationToken = default)
    {
        return await datasource.FindByNameAsync(pokemonName, cancellationToken);
    }

    public async Task<PokemonFileResult?> FindFileByIdAsync(ObjectId id,
        CancellationToken cancellationToken = default)
    {
        return await datasource.FindFileByIdAsync(id, cancellationToken);
    }
}