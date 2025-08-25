using FluentAssertions;
using MinhaAcademiaTEM.Application.Services.Helpers;
using MinhaAcademiaTEM.Domain.Interfaces;
using Moq;

namespace MinhaAcademiaTEM.UnitTests.Application.Helpers;

public class SlugGeneratorTests
{
    private readonly Mock<ICoachRepository> _coachRepository;
    private readonly SlugGenerator _slugGenerator;

    public SlugGeneratorTests()
    {
        _coachRepository = new Mock<ICoachRepository>();
        _slugGenerator = new SlugGenerator(_coachRepository.Object);
    }

    [Theory]
    [InlineData(" Rubem  Machado", "rubem-machado")]
    [InlineData("Rubem", "rubem")]
    [InlineData("Teste da Silva", "teste-da")]
    [InlineData("1234 da Silva", "1234-da")]
    [InlineData("João!!!   da  Silva", "joao-da")]
    [InlineData("ÁÉÍÓÚ ç  ñ", "aeiou-c")]
    public async Task GenerateUniqueSlugAsync_Should_Format_Base_Slug_When_Not_Exists(string name, string expected)
    {
        _coachRepository.Setup(r => r.ExistsSlugAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        var slug = await _slugGenerator.GenerateUniqueSlugAsync(name);

        slug.Should().Be(expected);
    }

    [Theory]
    [InlineData(1, "joao-da-2")]
    [InlineData(2, "joao-da-3")]
    [InlineData(3, "joao-da-4")]
    public async Task GenerateUniqueSlugAsync_Should_Append_Counter_On_Conflicts(int collisions, string expected)
    {
        var sequentialResult = _coachRepository.SetupSequence(r => r.ExistsSlugAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        for (int i = 1; i < collisions; i++)
            sequentialResult = sequentialResult.ReturnsAsync(true);

        sequentialResult.ReturnsAsync(false);

        var slug = await _slugGenerator.GenerateUniqueSlugAsync("João da Silva");

        slug.Should().Be(expected);
    }
}