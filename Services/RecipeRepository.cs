using CookHub.Models;

namespace CookHub.Services;

/// <summary>Implémentation des recettes et catégories au-dessus du fichier JSON local.</summary>
public class RecipeRepository : IRecipeRepository
{
    private static readonly string[] BaseCategories = { "Entrées", "Plats", "Desserts", "Boissons" };

    private readonly DataStore _store;

    public RecipeRepository(DataStore store)
    {
        _store = store;
    }

    public async Task<IReadOnlyList<Recipe>> GetAllAsync(string ownerEmail)
    {
        var data = await _store.GetAsync();
        return data.Recipes
            .Where(r => r.OwnerEmail == ownerEmail)
            .OrderByDescending(r => r.AddedOn)
            .ToList();
    }

    public async Task<Recipe?> GetAsync(Guid id)
    {
        var data = await _store.GetAsync();
        return data.Recipes.FirstOrDefault(r => r.Id == id);
    }

    public async Task AddAsync(Recipe recipe)
    {
        var data = await _store.GetAsync();
        data.Recipes.Add(recipe);
        await _store.SaveAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var data = await _store.GetAsync();
        data.Recipes.RemoveAll(r => r.Id == id);
        await _store.SaveAsync();
    }

    public async Task<IReadOnlyList<string>> GetCategoriesAsync()
    {
        var data = await _store.GetAsync();
        return BaseCategories.Concat(data.CustomCategories).ToList();
    }

    public async Task<bool> AddCategoryAsync(string name)
    {
        var clean = name.Trim();
        var data = await _store.GetAsync();

        var exists = BaseCategories.Concat(data.CustomCategories)
            .Any(c => string.Equals(c, clean, StringComparison.CurrentCultureIgnoreCase));
        if (clean.Length == 0 || exists)
        {
            return false;
        }

        data.CustomCategories.Add(clean);
        await _store.SaveAsync();
        return true;
    }
}
