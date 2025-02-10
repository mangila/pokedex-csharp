using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using pokedex_shared.Common.Attributes;
using pokedex_shared.Model.Domain;

namespace pokedex_unit_test.Model.Domain;

[TestFixture]
[TestOf(typeof(PokemonName))]
public class PokemonNameTest
{
    [Test]
    [SadPath]
    public void TestSpecialChars()
    {
        Action act = () => new PokemonName("#");
        act.Should().Throw<ValidationException>();
        act = () => new PokemonName("@");
        act.Should().Throw<ValidationException>();
    }

    [Test]
    [SadPath]
    public void TestExceptionNames()
    {
        var act = () => new PokemonName("Mr-mime");
        act.Should().NotThrow<ValidationException>();
        act = () => new PokemonName("porygon2");
        act.Should().NotThrow<ValidationException>();
        act = () => new PokemonName("minior-indigo-meteor");
        act.Should().NotThrow<ValidationException>();
        act = () => new PokemonName("zygarde-50");
        act.Should().NotThrow<ValidationException>();
    }

    [Test]
    [SadPath]
    public void TestLength()
    {
        var act = () => new PokemonName(string.Concat(Enumerable.Repeat("s", 100)));
        act.Should().NotThrow<ValidationException>();
        act = () => new PokemonName(string.Concat(Enumerable.Repeat("s", 101)));
        act.Should().Throw<ValidationException>();
    }
}