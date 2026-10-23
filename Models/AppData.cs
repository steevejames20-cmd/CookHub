namespace CookHub.Models;

/// <summary>Contenu complet du fichier de données local (cookhub.json).</summary>
public class AppData
{
    public List<AppUser> Users { get; set; } = new();
    public List<Recipe> Recipes { get; set; } = new();

    /// <summary>Catégories créées par l'utilisateur, en plus des quatre catégories de base.</summary>
    public List<string> CustomCategories { get; set; } = new();
}
