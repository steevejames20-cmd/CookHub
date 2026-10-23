using System.Globalization;
using System.Text;
using System.Text.Json.Serialization;

namespace CookHub.Models;

/// <summary>Une recette du carnet. Cette classe est enregistrée telle quelle en JSON.</summary>
public class Recipe
{
    private static readonly CultureInfo French = new("fr-FR");

    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>Adresse e-mail du propriétaire : chaque utilisateur a son propre carnet.</summary>
    public string OwnerEmail { get; set; } = string.Empty;

    public string AuthorName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Difficulty { get; set; } = "Facile";
    public int PrepMinutes { get; set; }
    public int CookMinutes { get; set; }
    public int Servings { get; set; } = 4;

    /// <summary>Chemin local de la photo (une seule photo par recette, compressée).</summary>
    public string? PhotoPath { get; set; }

    public DateTime AddedOn { get; set; } = DateTime.Now;
    public List<Ingredient> Ingredients { get; set; } = new();
    public List<string> Steps { get; set; } = new();

    // Propriétés calculées : non enregistrées dans le fichier JSON.

    [JsonIgnore]
    public int TotalMinutes => PrepMinutes + CookMinutes;

    /// <summary>Résumé affiché sur les cartes : "45 min · 4 personnes".</summary>
    [JsonIgnore]
    public string Summary => $"{TotalMinutes} min · {Servings} {(Servings > 1 ? "personnes" : "personne")}";

    [JsonIgnore]
    public string AddedLabel => $"Ajoutée par vous · {AddedOn.ToString("d MMMM", French)}";

    [JsonIgnore]
    public string AddedLongLabel => $"Ajoutée par {AuthorName} · {AddedOn.ToString("d MMMM yyyy", French)}";

    /// <summary>Identifiant lisible utilisé dans le lien de partage : "tarte-aux-tomates".</summary>
    [JsonIgnore]
    public string Slug => Slugify(Name);

    [JsonIgnore]
    public string ShareLink => $"cookhub.fr/r/{Slug}";

    /// <summary>Retire les accents et remplace tout caractère non alphanumérique par un tiret.</summary>
    private static string Slugify(string text)
    {
        var decomposed = text.Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder();
        foreach (var c in decomposed)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.NonSpacingMark)
            {
                continue;
            }

            builder.Append(char.IsLetterOrDigit(c) ? char.ToLowerInvariant(c) : '-');
        }

        var slug = string.Join('-', builder.ToString().Split('-', StringSplitOptions.RemoveEmptyEntries));
        return slug.Length == 0 ? "recette" : slug;
    }
}
