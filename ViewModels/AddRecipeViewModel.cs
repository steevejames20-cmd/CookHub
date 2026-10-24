using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookHub.Helpers;
using CookHub.Models;
using CookHub.Services;

namespace CookHub.ViewModels;

/// <summary>
/// Écran "Nouvelle recette" (Ajouter). Gère l'état "ajout avec erreurs" :
/// les champs en erreur sont signalés et toutes les autres informations sont conservées.
/// </summary>
public partial class AddRecipeViewModel : ObservableObject
{
    private readonly IAuthService _auth;
    private readonly IRecipeRepository _repository;

    public AddRecipeViewModel(IAuthService auth, IRecipeRepository repository)
    {
        _auth = auth;
        _repository = repository;
    }

    public ObservableCollection<string> Categories { get; } = new();

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private string? _category;

    [ObservableProperty]
    private string _prepMinutes = string.Empty;

    [ObservableProperty]
    private string _cookMinutes = string.Empty;

    [ObservableProperty]
    private string _servings = "4";

    [ObservableProperty]
    private string _description = string.Empty;

    [ObservableProperty]
    private string _ingredientsText = string.Empty;

    [ObservableProperty]
    private string _stepsText = string.Empty;

    [ObservableProperty]
    private string? _photoPath;

    // Messages d'erreur (états ERREUR de la charte)

    [ObservableProperty]
    private string _nameError = string.Empty;

    [ObservableProperty]
    private string _categoryError = string.Empty;

    [ObservableProperty]
    private string _stepsError = string.Empty;

    [ObservableProperty]
    private bool _showErrorBanner;

    [ObservableProperty]
    private bool _isSaving;

    partial void OnNameChanged(string value)
    {
        // L'erreur disparaît dès que l'utilisateur corrige le champ.
        if (!string.IsNullOrWhiteSpace(value))
        {
            NameError = string.Empty;
        }
    }

    partial void OnStepsTextChanged(string value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            StepsError = string.Empty;
        }
    }

    /// <summary>Recharge la liste des catégories (une catégorie a pu être créée entre-temps).</summary>
    [RelayCommand]
    private async Task LoadAsync()
    {
        var categories = await _repository.GetCategoriesAsync();
        Categories.Clear();
        foreach (var category in categories)
        {
            Categories.Add(category);
        }

        if (Category is null || !Categories.Contains(Category))
        {
            Category = Categories.Contains("Plats") ? "Plats" : Categories.FirstOrDefault();
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (!Validate())
        {
            return;
        }

        IsSaving = true;

        var recipe = new Recipe
        {
            OwnerEmail = _auth.CurrentEmail,
            AuthorName = _auth.CurrentFirstName,
            Name = Name.Trim(),
            Category = Category!,
            Description = Description.Trim(),
            PrepMinutes = ParseNumber(PrepMinutes, 0),
            CookMinutes = ParseNumber(CookMinutes, 0),
            Servings = Math.Max(1, ParseNumber(Servings, 4)),
            PhotoPath = PhotoPath,
            Ingredients = IngredientParser.ParseLines(IngredientsText),
            Steps = IngredientParser.SplitLines(StepsText),
            AddedOn = DateTime.Now,
        };

        await _repository.AddAsync(recipe);
        IsSaving = false;

        ResetForm();

        // Après enregistrement : on ouvre la recette qui vient d'être créée.
        await Shell.Current.GoToAsync($"recipe?id={recipe.Id}");
    }

    /// <summary>Vérifie les champs obligatoires. Retourne faux s'il y a au moins une erreur.</summary>
    private bool Validate()
    {
        NameError = string.IsNullOrWhiteSpace(Name) ? "Le nom de la recette est obligatoire." : string.Empty;
        CategoryError = string.IsNullOrWhiteSpace(Category) ? "Choisissez une catégorie." : string.Empty;
        StepsError = IngredientParser.SplitLines(StepsText).Count == 0 ? "Ajoutez au moins une étape." : string.Empty;

        ShowErrorBanner = NameError.Length > 0 || CategoryError.Length > 0 || StepsError.Length > 0;
        return !ShowErrorBanner;
    }

    private void ResetForm()
    {
        Name = string.Empty;
        PrepMinutes = string.Empty;
        CookMinutes = string.Empty;
        Servings = "4";
        Description = string.Empty;
        IngredientsText = string.Empty;
        StepsText = string.Empty;
        PhotoPath = null;
        NameError = string.Empty;
        CategoryError = string.Empty;
        StepsError = string.Empty;
        ShowErrorBanner = false;
    }

    private static int ParseNumber(string text, int fallback) =>
        int.TryParse(text.Trim(), out var value) && value >= 0 ? value : fallback;

    /// <summary>Choisit une photo dans la galerie puis la réduit avant de l'enregistrer.</summary>
    [RelayCommand]
    private async Task PickPhotoAsync()
    {
        try
        {
            var result = await MediaPicker.Default.PickPhotoAsync(new MediaPickerOptions { Title = "Choisir une photo" });
            if (result is null)
            {
                return;
            }

            PhotoPath = await PhotoCompressor.SaveCompressedAsync(result);
        }
        catch (Exception)
        {
            await Shell.Current.DisplayAlert("Photo", "La photo n'a pas pu être ajoutée. Vos autres informations sont conservées.", "OK");
        }
    }
}
