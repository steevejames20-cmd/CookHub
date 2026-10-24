using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookHub.Helpers;
using CookHub.Models;
using CookHub.Services;

namespace CookHub.ViewModels;

/// <summary>
/// Écran "Mon carnet de recettes" avec ses trois états :
/// chargement (squelette), carnet vide et liste de recettes.
/// </summary>
public partial class CarnetViewModel : ObservableObject
{
    private const string AllFilter = "Tout";

    private readonly IAuthService _auth;
    private readonly IRecipeRepository _repository;

    // Toutes les recettes de l'utilisateur ; la liste affichée en est un sous-ensemble filtré.
    private List<Recipe> _all = new();

    public CarnetViewModel(IAuthService auth, IRecipeRepository repository)
    {
        _auth = auth;
        _repository = repository;
    }

    public ObservableCollection<Recipe> Recipes { get; } = new();
    public ObservableCollection<ChipItem> Filters { get; } = new();

    [ObservableProperty]
    private string _greeting = string.Empty;

    [ObservableProperty]
    private string _subtitle = string.Empty;

    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private string _selectedFilter = AllFilter;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SortLabel))]
    private bool _newestFirst = true;

    // Les trois états de l'écran : un seul est visible à la fois.
    [ObservableProperty]
    private bool _showLoading = true;

    [ObservableProperty]
    private bool _showEmpty;

    [ObservableProperty]
    private bool _showContent;

    /// <summary>Vrai quand un filtre ou une recherche ne retourne aucune recette.</summary>
    [ObservableProperty]
    private bool _showNoMatch;

    public string SortLabel => NewestFirst ? "Plus récentes" : "Plus anciennes";

    partial void OnSearchTextChanged(string value) => ApplyFilters();

    partial void OnNewestFirstChanged(bool value) => ApplyFilters();

    /// <summary>Charge le carnet. Appelée à chaque affichage de l'onglet pour rester à jour.</summary>
    [RelayCommand]
    private async Task LoadAsync()
    {
        ShowLoading = true;
        ShowEmpty = false;
        ShowContent = false;
        Greeting = $"Bonjour {_auth.CurrentFirstName}";

        // Courte pause pour que l'état de chargement soit perceptible (démonstration du squelette).
        await Task.Delay(400);

        _all = (await _repository.GetAllAsync(_auth.CurrentEmail)).ToList();
        var categories = await _repository.GetCategoriesAsync();

        // Reconstruit les puces de filtre en conservant la sélection courante.
        if (SelectedFilter != AllFilter && !categories.Contains(SelectedFilter))
        {
            SelectedFilter = AllFilter;
        }

        Filters.Clear();
        Filters.Add(new ChipItem(AllFilter, SelectedFilter == AllFilter));
        foreach (var category in categories)
        {
            Filters.Add(new ChipItem(category, SelectedFilter == category));
        }

        ApplyFilters();

        Subtitle = _all.Count == 0
            ? "Vos prochaines bonnes idées commencent ici."
            : $"{_all.Count} {(_all.Count > 1 ? "recettes" : "recette")} à garder, à refaire, à partager.";

        ShowLoading = false;
        ShowEmpty = _all.Count == 0;
        ShowContent = _all.Count > 0;
    }

    /// <summary>Applique la recherche, le filtre de catégorie et l'ordre de tri.</summary>
    private void ApplyFilters()
    {
        IEnumerable<Recipe> query = _all;

        if (SelectedFilter != AllFilter)
        {
            query = query.Where(r => r.Category == SelectedFilter);
        }

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            query = query.Where(r => TextMatch.Contains(r.Name, SearchText));
        }

        query = NewestFirst ? query.OrderByDescending(r => r.AddedOn) : query.OrderBy(r => r.AddedOn);

        Recipes.Clear();
        foreach (var recipe in query)
        {
            Recipes.Add(recipe);
        }

        ShowNoMatch = _all.Count > 0 && Recipes.Count == 0;
    }

    [RelayCommand]
    private void SelectFilter(ChipItem chip)
    {
        SelectedFilter = chip.Label;
        foreach (var item in Filters)
        {
            item.IsSelected = item == chip;
        }

        ApplyFilters();
    }

    [RelayCommand]
    private void ToggleSort() => NewestFirst = !NewestFirst;

    [RelayCommand]
    private async Task OpenRecipeAsync(Recipe recipe)
    {
        await Shell.Current.GoToAsync($"recipe?id={recipe.Id}");
    }

    [RelayCommand]
    private async Task AddFirstRecipeAsync()
    {
        await Shell.Current.GoToAsync("//main/ajouter");
    }

    /// <summary>Menu du compte (icône en haut à droite).</summary>
    [RelayCommand]
    private async Task OpenAccountMenuAsync()
    {
        var themeAction = ThemeService.IsDark ? "Passer au thème clair" : "Passer au thème sombre";
        var choice = await Shell.Current.DisplayActionSheet(
            $"Compte de {_auth.CurrentFirstName}", "Annuler", null, themeAction, "Se déconnecter");

        if (choice == themeAction)
        {
            ThemeService.SetDark(!ThemeService.IsDark);
            return;
        }

        if (choice == "Se déconnecter")
        {
            _auth.Logout();
            await Shell.Current.GoToAsync("//login");
        }
    }
}
