using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using pokedex_shared.Model.Domain;

namespace pokedex_unit_test.Model.Domain;

[TestFixture]
[TestOf(typeof(PokemonName))]
public class PokemonNameTest
{
    [Test]
    public void Test()
    {
        // No special stuffs
        Action act = () => new PokemonName("#");
        act.Should().Throw<ValidationException>();
        // Except Mr.Mime
        act = () => new PokemonName("Mr.Mime");
        act.Should().NotThrow<ValidationException>();
        // 49 chars
        act = () => new PokemonName(string.Concat(Enumerable.Repeat("s", 49)));
        act.Should().NotThrow<ValidationException>();
        // 51 chars
        act = () => new PokemonName(string.Concat(Enumerable.Repeat("s", 51)));
        act.Should().Throw<ValidationException>();
    }
}