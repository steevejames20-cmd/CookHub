using CookHub.ViewModels;

namespace CookHub.Views;

/// <summary>Écran d'inscription. Toute la logique est dans RegisterViewModel.</summary>
public partial class RegisterPage : ContentPage
{
    public RegisterPage(RegisterViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
