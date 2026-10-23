using CookHub.ViewModels;

namespace CookHub.Views;

/// <summary>Onglet Carnet : recharge les données à chaque affichage.</summary>
public partial class CarnetPage : ContentPage
{
    private readonly CarnetViewModel _viewModel;

    public CarnetPage(CarnetViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Rechargement à chaque retour sur l'onglet : une recette ajoutée ailleurs apparaît aussitôt.
        await _viewModel.LoadCommand.ExecuteAsync(null);
    }
}
