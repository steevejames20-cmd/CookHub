using CookHub.ViewModels;

namespace CookHub.Views;

/// <summary>Partage d'une recette. L'identifiant arrive par la navigation (voir ShareViewModel).</summary>
public partial class SharePage : ContentPage
{
    public SharePage(ShareViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
