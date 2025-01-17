
using pokedex_api.Model;

namespace pokedex_shared.Service;

public class PokemonService(DatasourceService datasource)
{
    public IEnumerable<PokemonResponse> FindAllByIdAsync(List<string> ids,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<PokemonResponse> FindAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public PokemonResponse FindOneByAsync(string value, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}