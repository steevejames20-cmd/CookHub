using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookHub.Models;
using CookHub.Services;

namespace CookHub.ViewModels;

/// <summary>
/// Écran "Détail de recette" : photo, bandeau de durées, ingrédients et étapes.
/// La recette à afficher arrive par la navigation : "recipe?id=...".
/// </summary>
public partial class DetailViewModel : ObservableObject, IQueryAttributable
{
    private readonly IRecipeRepository _repository;
    private Recipe? _recipe;

    public DetailViewModel(IRecipeRepository repository)
    {
        _repository = repository;
    }

    public ObservableCollection<IngredientItem> Ingredients { get; } = new();
    public ObservableCollection<StepItem> Steps { get; } = new();

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private string _category = string.Empty;

    [ObservableProperty]
    private string _difficulty = string.Empty;

    [ObservableProperty]
    private string _description = string.Empty;

    [ObservableProperty]
    private string _addedLabel = string.Empty;

    [ObservableProperty]
    private string? _photoPath;

    [ObservableProperty]
    private string _prepText = string.Empty;

    [ObservableProperty]
    private string _cookText = string.Empty;

    [ObservableProperty]
    private string _servingsText = string.Empty;

    public string ServingsLabel => $"Pour {_recipe?.Servings ?? 0} personnes";

    /// <summary>Reçoit l'identifiant de la recette depuis la route de navigation.</summary>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("id", out var value) && Guid.TryParse(value?.ToString(), out var id))
        {
            _ = LoadAsync(id);
        }
    }

    private async Task LoadAsync(Guid id)
    {
        _recipe = await _repository.GetAsync(id);
        if (_recipe is null)
        {
            return;
        }

        Name = _recipe.Name;
        Category = _recipe.Category;
        Difficulty = _recipe.Difficulty;
        Description = _recipe.Description;
        AddedLabel = _recipe.AddedLongLabel;
        PhotoPath = _recipe.PhotoPath;
        PrepText = $"{_recipe.PrepMinutes} min";
        CookText = $"{_recipe.CookMinutes} min";
        ServingsText = $"{_recipe.Servings} pers.";

        Ingredients.Clear();
        foreach (var ingredient in _recipe.Ingredients)
        {
            Ingredients.Add(new IngredientItem(ingredient));
        }

        Steps.Clear();
        for (var i = 0; i < _recipe.Steps.Count; i++)
        {
            Steps.Add(new StepItem(i + 1, _recipe.Steps[i]));
        }

        OnPropertyChanged(nameof(ServingsLabel));
    }

}
