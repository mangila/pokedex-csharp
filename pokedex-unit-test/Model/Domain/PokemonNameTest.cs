using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using pokedex_shared.Model.Domain;

namespace pokedex_unit_test.Model.Domain;

[TestFixture]
[TestOf(typeof(PokemonName))]
public class PokemonNameTest
{
    [Test]
    public void TestSpecialChars()
    {
        Action act = () => new PokemonName("#");
        act.Should().Throw<ValidationException>();
        act = () => new PokemonName("@");
        act.Should().Throw<ValidationException>();
        act = () => new PokemonName("123");
        act.Should().Throw<ValidationException>();
        act = () => new PokemonName("2");
        act.Should().Throw<ValidationException>();
    }

    [Test]
    public void TestExceptionNames()
    {
        var act = () => new PokemonName("Mr-mime");
        act.Should().NotThrow<ValidationException>();
        act = () => new PokemonName("porygon2");
        act.Should().NotThrow<ValidationException>();
    }

    [Test]
    public void TestLength()
    {
        var act = () => new PokemonName(string.Concat(Enumerable.Repeat("s", 50)));
        act.Should().NotThrow<ValidationException>();
        act = () => new PokemonName(string.Concat(Enumerable.Repeat("s", 51)));
        act.Should().Throw<ValidationException>();
    }
}