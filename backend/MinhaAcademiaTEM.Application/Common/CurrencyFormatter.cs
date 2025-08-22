using System.Globalization;

namespace MinhaAcademiaTEM.Application.Common;

public static class CurrencyFormatter
{
    public static string Format(long amountInCents, string currency)
    {
        var amount = amountInCents / 100m;
        var code = currency.ToUpperInvariant();

        var culture = code switch
        {
            "EUR" => new CultureInfo("pt-PT"),
            "USD" => new CultureInfo("en-US"),
            "BRL" => new CultureInfo("pt-BR"),
            _ => CultureInfo.InvariantCulture
        };

        var symbol = code switch
        {
            "EUR" => "€",
            "USD" => "$",
            "BRL" => "R$",
            _ => code
        };

        return $"{symbol} {amount.ToString("N2", culture)}";
    }
}