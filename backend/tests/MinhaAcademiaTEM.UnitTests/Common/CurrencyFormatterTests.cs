using FluentAssertions;
using MinhaAcademiaTEM.Application.Common;

namespace MinhaAcademiaTEM.UnitTests.Common;

public class CurrencyFormatterTests
{
    [Theory]
    [InlineData(12345, "EUR", "€ 123,45")]
    [InlineData(12345, "USD", "$ 123.45")]
    [InlineData(12345, "BRL", "R$ 123,45")]
    [InlineData(12345, "GBP", "GBP 123.45")]
    public void Format_Should_Format_By_Currency(long cents, string currency, string expected)
    {
        var result = CurrencyFormatter.Format(cents, currency);

        result.Should().Be(expected);
    }
}