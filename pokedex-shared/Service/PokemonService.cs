using System.Collections.Immutable;
using MongoDB.Bson;
using pokedex_shared.Model.Document;
using pokedex_shared.Model.Domain;
using pokedex_shared.Model.Dto;

namespace pokedex_shared.Service;

public class PokemonService(DatasourceQueryService datasourceQuery)
{
    public async Task<ImmutableList<PokemonSpeciesDto>> FindAllByIdsAsync(
        PokemonIdCollection pokemonIdCollection,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var collection = await datasourceQuery
            .FindAllByIdsAsync(pokemonIdCollection, cancellationToken);
        return collection.ToDtos();
    }

    public async Task<PaginationResultDto> FindByPaginationAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var paginationResult = await datasourceQuery.FindByPaginationAsync(page, pageSize, cancellationToken);
        return paginationResult.ToDto();
    }

    public async Task<ImmutableList<PokemonSpeciesDto>> SearchByNameAsync(
        PokemonName search,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var collection = await datasourceQuery
            .SearchByNameAsync(search, cancellationToken);
        return collection.ToDtos();
    }

    public async Task<ImmutableList<PokemonSpeciesDto>> SearchByGenerationAsync(
        PokemonGeneration generation,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var collection = await datasourceQuery
            .SearchByGenerationAsync(generation, cancellationToken);
        return collection.ToDtos();
    }

    public async Task<PokemonSpeciesDto> FindOneByIdAsync(
        PokemonId pokemonId,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var document = await datasourceQuery
            .FindOneByIdAsync(pokemonId, cancellationToken);
        return document.Equals(default) ? default : document.ToDto();
    }

    public async Task<PokemonSpeciesDto> FindOneByNameAsync(
        PokemonName pokemonName,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var document = await datasourceQuery
            .FindByNameAsync(pokemonName, cancellationToken);
        return document.Equals(default) ? default : document.ToDto();
    }

    public async Task<PokemonFileResult?> FindFileByIdAsync(
        ObjectId id,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await datasourceQuery.FindFileByIdAsync(id, cancellationToken);
    }
}