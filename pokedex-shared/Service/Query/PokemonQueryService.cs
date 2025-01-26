using MongoDB.Bson;
using pokedex_shared.Model.Document;
using pokedex_shared.Model.Document.Projection;
using pokedex_shared.Model.Domain;
using pokedex_shared.Model.Dto;
using pokedex_shared.Model.Dto.Collection;

namespace pokedex_shared.Service.Query;

public class PokemonQueryService(DatasourceQueryService datasourceQuery)
{
    public async Task<PokemonMediaProjectionDtoCollection> FindAllByPokemonIdAsync(
        PokemonIdCollection pokemonIdCollection,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var collection = await datasourceQuery
            .FindAllByPokemonIdAsync(pokemonIdCollection, cancellationToken);
        return collection.ToDtoCollection();
    }

    public async Task<PokemonMediaProjectionDtoCollection> FindAllAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var collection = await datasourceQuery
            .FindAllAsync(cancellationToken);
        return collection.ToDtoCollection();
    }

    public async Task<PokemonMediaProjectionDtoCollection> SearchByNameAsync(PokemonName search,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var collection = await datasourceQuery
            .SearchByNameAsync(search, cancellationToken);
        return collection.ToDtoCollection();
    }

    public async Task<PokemonMediaProjectionDtoCollection> SearchByGenerationAsync(PokemonGeneration generation,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var collection = await datasourceQuery
            .SearchByGenerationAsync(generation, cancellationToken);
        return collection.ToDtoCollection();
    }

    public async Task<PokemonDto> FindOneByPokemonIdAsync(PokemonId pokemonId,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var document = await datasourceQuery
            .FindByPokemonIdAsync(pokemonId, cancellationToken);
        return document.Equals(default) ? default : document.ToDto();
    }

    public async Task<PokemonDto> FindOneByNameAsync(PokemonName pokemonName,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var document = await datasourceQuery
            .FindByNameAsync(pokemonName, cancellationToken);
        return document.Equals(default) ? default : document.ToDto();
    }

    public async Task<PokemonFileResult?> FindFileByIdAsync(ObjectId id,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await datasourceQuery.FindFileByIdAsync(id, cancellationToken);
    }
}