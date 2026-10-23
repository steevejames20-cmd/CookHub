namespace CookHub.Models;

/// <summary>Une catégorie affichée dans l'onglet Catégories, avec son nombre de recettes.</summary>
public record CategoryItem(string Name, string CountText, bool HasRecipes, string? PhotoPath);
