using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using pokedex_shared.Model.Domain;

namespace pokedex_unit_test.Model.Domain;

[TestFixture]
[TestOf(typeof(PokemonId))]
public class PokemonIdTest
{
    [Test]
    public void Test()
    {
        // No big numbers
        Action act = () => new PokemonId(100000);
        act.Should().Throw<ValidationException>();
        // Must be a number
        act = () => new PokemonId("s");
        act.Should().Throw<ValidationException>();
    }
}