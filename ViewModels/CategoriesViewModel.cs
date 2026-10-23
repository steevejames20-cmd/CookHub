using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookHub.Models;
using CookHub.Services;

namespace CookHub.ViewModels;

/// <summary>Écran "Catégories" : les catégories du carnet, leur nombre de recettes, et la création d'une catégorie.</summary>
public partial class CategoriesViewModel : ObservableObject
{
    private readonly IAuthService _auth;
    private readonly IRecipeRepository _repository;

    public CategoriesViewModel(IAuthService auth, IRecipeRepository repository)
    {
        _auth = auth;
        _repository = repository;
    }

    public ObservableCollection<CategoryItem> Items { get; } = new();

    [ObservableProperty]
    private string _subtitle = string.Empty;

    [RelayCommand]
    private async Task LoadAsync()
    {
        var recipes = await _repository.GetAllAsync(_auth.CurrentEmail);
        var categories = await _repository.GetCategoriesAsync();

        Items.Clear();
        foreach (var name in categories)
        {
            var inCategory = recipes.Where(r => r.Category == name).ToList();
            var count = inCategory.Count;
            var text = count switch
            {
                0 => "Aucune recette pour le moment",
                1 => "1 recette",
                _ => $"{count} recettes",
            };

            // La vignette de la catégorie est la photo de sa recette la plus récente.
            var photo = inCategory.FirstOrDefault(r => !string.IsNullOrEmpty(r.PhotoPath))?.PhotoPath;
            Items.Add(new CategoryItem(name, text, count > 0, photo));
        }

        Subtitle = recipes.Count switch
        {
            0 => "Aucune recette pour le moment.",
            1 => "1 recette, soigneusement rangée.",
            _ => $"{recipes.Count} recettes, soigneusement rangées.",
        };
    }

    /// <summary>Ouvre la recherche déjà filtrée sur la catégorie touchée.</summary>
    [RelayCommand]
    private Task OpenCategoryAsync(CategoryItem item) =>
        Shell.Current.GoToAsync($"//main/recherche?category={Uri.EscapeDataString(item.Name)}");

    [RelayCommand]
    private async Task CreateCategoryAsync()
    {
        var name = await Shell.Current.DisplayPromptAsync(
            "Nouvelle catégorie", "Donnez un nom à votre catégorie.", "Créer", "Annuler",
            "Ex. Apéritifs", maxLength: 30);

        // null = l'utilisateur a annulé.
        if (name is null)
        {
            return;
        }

        if (!await _repository.AddCategoryAsync(name))
        {
            await Shell.Current.DisplayAlert("Catégorie", "Ce nom est vide ou existe déjà.", "OK");
            return;
        }

        await LoadAsync();
    }
}
