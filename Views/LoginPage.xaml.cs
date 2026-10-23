using CookHub.ViewModels;

namespace CookHub.Views;

/// <summary>Écran de connexion. Toute la logique est dans LoginViewModel.</summary>
public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
