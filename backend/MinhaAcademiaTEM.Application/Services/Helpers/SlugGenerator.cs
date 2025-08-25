using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using MinhaAcademiaTEM.Domain.Interfaces;

namespace MinhaAcademiaTEM.Application.Services.Helpers;

public class SlugGenerator(ICoachRepository coachRepository)
{
    public async Task<string> GenerateUniqueSlugAsync(string name)
    {
        var baseSlug = GenerateSlugBase(name);
        var slug = baseSlug;
        int counter = 2;

        while (await coachRepository.ExistsSlugAsync(slug))
        {
            slug = $"{baseSlug}-{counter}";
            counter++;
        }

        return slug;
    }

    private static string GenerateSlugBase(string name)
    {
        string normalized = name.ToLowerInvariant().Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();

        foreach (char c in normalized)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);

            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }

        string cleaned = sb.ToString().Normalize(NormalizationForm.FormC);
        cleaned = Regex.Replace(cleaned, @"[^a-z0-9\s-]", "");
        cleaned = Regex.Replace(cleaned, @"\s+", " ").Trim();

        var words = cleaned.Split(' ').Take(2);
        var slug = string.Join("-", words);

        return Regex.Replace(slug, @"-+", "-").Trim('-');
    }
}