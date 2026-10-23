using System.Globalization;
using System.Text.RegularExpressions;
using CookHub.Models;

namespace CookHub.Helpers;

/// <summary>
/// Transforme le texte saisi (une ligne par ingrédient) en objets Ingredient.
/// Exemples : "4 tomates mûres", "2 c. à soupe moutarde", "80 g comté râpé", "sel".
/// </summary>
public static class IngredientParser
{
    // Unités reconnues. Les plus longues sont testées en premier ("c. à soupe" avant "c").
    private static readonly string[] KnownUnits =
    {
        "c. à soupe", "c. à café", "kg", "g", "cl", "ml", "l", "pincée", "pincées",
        "tasse", "tasses", "verre", "verres",
    };

    // Une quantité au début de la ligne : "4", "80", "1,5" ou "1.5".
    private static readonly Regex LeadingNumber = new(@"^\s*(\d+(?:[.,]\d+)?)\s*(.*)$", RegexOptions.Compiled);

    public static Ingredient Parse(string line)
    {
        var match = LeadingNumber.Match(line);
        if (!match.Success)
        {
            return new Ingredient { Name = line.Trim() };
        }

        double.TryParse(match.Groups[1].Value.Replace(',', '.'), NumberStyles.Float,
            CultureInfo.InvariantCulture, out var amount);
        var rest = match.Groups[2].Value.Trim();

        foreach (var unit in KnownUnits.OrderByDescending(u => u.Length))
        {
            // L'unité doit être suivie d'un espace pour ne pas couper un mot ("gousses" n'est pas "g").
            if (rest.StartsWith(unit + " ", StringComparison.OrdinalIgnoreCase))
            {
                return new Ingredient { Amount = amount, Unit = unit, Name = rest[unit.Length..].Trim() };
            }
        }

        return new Ingredient { Amount = amount, Name = rest };
    }

    /// <summary>Découpe un texte multiligne et ignore les lignes vides.</summary>
    public static List<Ingredient> ParseLines(string text) =>
        SplitLines(text).Select(Parse).ToList();

    /// <summary>Découpe un texte multiligne en lignes non vides (utilisé aussi pour les étapes).</summary>
    public static List<string> SplitLines(string text) =>
        text.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
}
