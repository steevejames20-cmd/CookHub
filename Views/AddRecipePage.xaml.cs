using CookHub.ViewModels;

namespace CookHub.Views;

/// <summary>Onglet Ajouter : recharge les catégories à chaque affichage.</summary>
public partial class AddRecipePage : ContentPage
{
    private readonly AddRecipeViewModel _viewModel;

    public AddRecipePage(AddRecipeViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadCommand.ExecuteAsync(null);
    }
}
