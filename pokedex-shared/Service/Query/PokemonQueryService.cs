using MongoDB.Bson;
using pokedex_shared.Model.Domain;
using pokedex_shared.Model.Dto;

namespace pokedex_shared.Service.Query;

public class PokemonQueryService(DatasourceQueryService datasourceQuery)
{
    public async Task<PokemonNameImagesDtoCollection> FindAllByPokemonIdAsync(PokemonIdCollection pokemonIdCollection,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await datasourceQuery.FindAllByPokemonIdAsync(pokemonIdCollection, cancellationToken);
    }

    public async Task<PokemonNameImagesDtoCollection> FindAllAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await datasourceQuery.FindAllAsync(cancellationToken);
    }

    public async Task<PokemonNameImagesDtoCollection> SearchByNameAsync(PokemonName search,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await datasourceQuery.SearchByNameAsync(search, cancellationToken);
    }

    public async Task<PokemonNameImagesDtoCollection> SearchByGenerationAsync(PokemonGeneration generation,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await datasourceQuery.SearchByGenerationAsync(generation, cancellationToken);
    }

    public async Task<PokemonDto?> FindOneByPokemonIdAsync(PokemonId pokemonId,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await datasourceQuery.FindByPokemonIdAsync(pokemonId, cancellationToken);
    }

    public async Task<PokemonDto?> FindOneByNameAsync(PokemonName pokemonName,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await datasourceQuery.FindByNameAsync(pokemonName, cancellationToken);
    }

    public async Task<PokemonFileResult?> FindFileByIdAsync(ObjectId id,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await datasourceQuery.FindFileByIdAsync(id, cancellationToken);
    }
}