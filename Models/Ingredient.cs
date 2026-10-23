namespace CookHub.Models;

/// <summary>Un ingrédient d'une recette : quantité, unité (facultative) et nom.</summary>
public class Ingredient
{
    /// <summary>Quantité pour le nombre de personnes de la recette (0 si non précisée).</summary>
    public double Amount { get; set; }

    /// <summary>Unité, par exemple "g" ou "c. à soupe". Vide pour un ingrédient comptable.</summary>
    public string Unit { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
}
