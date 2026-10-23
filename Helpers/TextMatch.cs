using System.Globalization;

namespace CookHub.Helpers;

/// <summary>Recherche de texte tolérante : sans distinction de casse ni d'accents.</summary>
public static class TextMatch
{
    /// <summary>Vrai si <paramref name="source"/> contient <paramref name="term"/> ("tomate" trouve "Tomâtes").</summary>
    public static bool Contains(string? source, string? term)
    {
        if (string.IsNullOrEmpty(term))
        {
            return true;
        }

        if (string.IsNullOrEmpty(source))
        {
            return false;
        }

        return CultureInfo.InvariantCulture.CompareInfo.IndexOf(
            source, term.Trim(), CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0;
    }
}
