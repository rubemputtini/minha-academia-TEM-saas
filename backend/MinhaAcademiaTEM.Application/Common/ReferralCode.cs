namespace MinhaAcademiaTEM.Application.Common;

public static class ReferralCode
{
    public static string FromSlug(string slug)
    {
        var code = System.Text.RegularExpressions.Regex.Replace(slug, "[^a-zA-Z0-9]", "");

        return code.ToUpperInvariant();
    }
}