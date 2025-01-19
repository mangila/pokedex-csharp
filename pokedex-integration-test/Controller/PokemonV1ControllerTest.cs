using pokedex_api.Controller;
using pokedex_shared.Model.Domain;

namespace pokedex_integration_test.Controller;

[TestFixture]
[TestOf(typeof(PokemonV1Controller))]
public class PokemonV1ControllerTest
{
    [Test]
    public void METHOD()
    {
        new PokemonId("hej");
    }
}