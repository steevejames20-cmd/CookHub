using CookHub.Models;

namespace CookHub.Services;

/// <summary>Accès aux recettes et aux catégories.</summary>
public interface IRecipeRepository
{
    /// <summary>Toutes les recettes d'un utilisateur, de la plus récente à la plus ancienne.</summary>
    Task<IReadOnlyList<Recipe>> GetAllAsync(string ownerEmail);

    Task<Recipe?> GetAsync(Guid id);
    Task AddAsync(Recipe recipe);
    Task DeleteAsync(Guid id);

    /// <summary>Catégories de base (Entrées, Plats, Desserts, Boissons) puis catégories créées.</summary>
    Task<IReadOnlyList<string>> GetCategoriesAsync();

    /// <summary>Crée une catégorie. Retourne faux si le nom est vide ou déjà utilisé.</summary>
    Task<bool> AddCategoryAsync(string name);
}
