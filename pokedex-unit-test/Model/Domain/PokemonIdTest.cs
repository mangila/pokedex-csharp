using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using pokedex_shared.Common.Attributes;
using pokedex_shared.Model.Domain;

namespace pokedex_unit_test.Model.Domain;

[TestFixture]
[TestOf(typeof(PokemonId))]
public class PokemonIdTest
{
    [Test]
    [SadPath]
    public void TestBigNumber()
    {
        Action act = () => new PokemonId(100000);
        act.Should().Throw<ValidationException>();
    }

    [Test]
    [SadPath]
    public void TestInvalidNumber()
    {
        Action act = () => new PokemonId("s");
        act.Should().Throw<ValidationException>();
    }
}