using CookHub.ViewModels;

namespace CookHub.Views;

/// <summary>Détail d'une recette. L'identifiant arrive par la navigation (voir DetailViewModel).</summary>
public partial class DetailPage : ContentPage
{
    public DetailPage(DetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
