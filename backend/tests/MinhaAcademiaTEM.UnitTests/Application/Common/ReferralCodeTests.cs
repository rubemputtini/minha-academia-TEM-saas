using FluentAssertions;
using MinhaAcademiaTEM.Application.Common;

namespace MinhaAcademiaTEM.UnitTests.Application.Common;

public class ReferralCodeTests
{
    [Theory]
    [InlineData("rubem-machado", "RUBEMMACHADO")]
    [InlineData("coach.123", "COACH123")]
    [InlineData("rübêm", "RBM")]
    [InlineData("----", "")]
    public void FromSlug_Should_Keep_Alphanumerics_And_Uppercase(string input, string expected)
    {
        var code = ReferralCode.FromSlug(input);
        code.Should().Be(expected);
    }
}