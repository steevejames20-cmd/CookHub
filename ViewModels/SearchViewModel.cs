using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookHub.Helpers;
using CookHub.Models;
using CookHub.Services;

namespace CookHub.ViewModels;

/// <summary>
/// Écran "Recherche et filtres" : recherche par nom de recette ou par ingrédient,
/// combinée à un filtre de catégorie. Gère l'état "recherche sans résultat".
/// </summary>
public partial class SearchViewModel : ObservableObject, IQueryAttributable
{
    private const string AllCategories = "Tout";

    private readonly IAuthService _auth;
    private readonly IRecipeRepository _repository;
    private List<Recipe> _all = new();

    public SearchViewModel(IAuthService auth, IRecipeRepository repository)
    {
        _auth = auth;
        _repository = repository;
    }

    public ObservableCollection<Recipe> Results { get; } = new();
    public ObservableCollection<ChipItem> Categories { get; } = new();

    [ObservableProperty]
    private string _query = string.Empty;

    [ObservableProperty]
    private string _selectedCategory = AllCategories;

    [ObservableProperty]
    private string _scopeText = "Dans votre carnet";

    [ObservableProperty]
    private string _resultCountText = string.Empty;

    [ObservableProperty]
    private string _noResultTitle = string.Empty;

    [ObservableProperty]
    private string _noResultText = string.Empty;

    [ObservableProperty]
    private bool _showResults = true;

    [ObservableProperty]
    private bool _showNoResult;

    partial void OnQueryChanged(string value) => Refresh();

    /// <summary>Reçoit la catégorie choisie depuis l'onglet Catégories (route "recherche?category=Plats").</summary>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("category", out var value) && value is string category && category.Length > 0)
        {
            SelectedCategory = category;
        }
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        _all = (await _repository.GetAllAsync(_auth.CurrentEmail)).ToList();
        var categories = await _repository.GetCategoriesAsync();

        if (SelectedCategory != AllCategories && !categories.Contains(SelectedCategory))
        {
            SelectedCategory = AllCategories;
        }

        Categories.Clear();
        Categories.Add(new ChipItem(AllCategories, SelectedCategory == AllCategories));
        foreach (var category in categories)
        {
            Categories.Add(new ChipItem(category, SelectedCategory == category));
        }

        Refresh();
    }

    [RelayCommand]
    private void SelectCategory(ChipItem chip)
    {
        SelectedCategory = chip.Label;
        foreach (var item in Categories)
        {
            item.IsSelected = item == chip;
        }

        Refresh();
    }

    /// <summary>Lien "Réinitialiser" : efface la recherche et le filtre.</summary>
    [RelayCommand]
    private void Reset()
    {
        Query = string.Empty;
        SelectedCategory = AllCategories;
        foreach (var item in Categories)
        {
            item.IsSelected = item.Label == AllCategories;
        }

        Refresh();
    }

    /// <summary>Bouton "Effacer la recherche" de l'état sans résultat : garde la catégorie.</summary>
    [RelayCommand]
    private void ClearQuery() => Query = string.Empty;

    [RelayCommand]
    private async Task ShowAllCategoriesAsync()
    {
        Reset();
        await Task.CompletedTask;
    }

    [RelayCommand]
    private async Task OpenRecipeAsync(Recipe recipe)
    {
        await Shell.Current.GoToAsync($"recipe?id={recipe.Id}");
    }

    /// <summary>Recalcule la liste des résultats selon le texte saisi et la catégorie choisie.</summary>
    private void Refresh()
    {
        var text = Query.Trim();
        var inCategory = SelectedCategory != AllCategories;

        IEnumerable<Recipe> matches = _all;
        if (inCategory)
        {
            matches = matches.Where(r => r.Category == SelectedCategory);
        }

        if (text.Length > 0)
        {
            // Une recette correspond si son nom ou l'un de ses ingrédients contient le texte.
            matches = matches.Where(r =>
                TextMatch.Contains(r.Name, text) || r.Ingredients.Any(i => TextMatch.Contains(i.Name, text)));
        }

        Results.Clear();
        foreach (var recipe in matches)
        {
            Results.Add(recipe);
        }

        ScopeText = inCategory ? $"Dans votre carnet · Catégorie : {SelectedCategory}" : "Dans votre carnet";
        ResultCountText = Results.Count > 1 ? $"{Results.Count} recettes trouvées" : $"{Results.Count} recette trouvée";

        ShowNoResult = Results.Count == 0 && (text.Length > 0 || inCategory);
        ShowResults = !ShowNoResult;

        if (ShowNoResult)
        {
            var place = inCategory ? $" dans {SelectedCategory}" : string.Empty;
            NoResultTitle = text.Length > 0 ? $"Pas encore de {text}." : $"Rien dans {SelectedCategory} pour l'instant.";
            NoResultText = text.Length > 0
                ? $"Aucune recette ne correspond à « {text} »{place}. Essayez un autre mot ou élargissez vos catégories."
                : "Cette catégorie ne contient pas encore de recette. Ajoutez-en une depuis l'onglet Ajouter.";
        }
    }
}
