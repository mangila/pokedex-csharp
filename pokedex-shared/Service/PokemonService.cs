using pokedex_shared.Model;

namespace pokedex_shared.Service;

public class PokemonService(DatasourceService datasource)
{
    public IEnumerable<PokemonDto> FindAllByIdAsync(List<string> ids,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<PokemonDto> FindAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public PokemonDto FindOneByAsync(string value, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}