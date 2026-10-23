using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookHub.Helpers;
using CookHub.Services;

namespace CookHub.ViewModels;

/// <summary>
/// Logique de l'écran de connexion.
/// Les attributs [ObservableProperty] et [RelayCommand] génèrent automatiquement
/// les propriétés (Email, Password...) et les commandes (LoginCommand...) utilisées en XAML.
/// </summary>
public partial class LoginViewModel : ObservableObject
{
    private readonly IAuthService _auth;

    public LoginViewModel(IAuthService auth)
    {
        _auth = auth;
    }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    [NotifyPropertyChangedFor(nameof(EmailValid))]
    private string _email = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    private string _password = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasError))]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    private bool _isBusy;

    public bool EmailValid => Validators.IsEmail(Email);

    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    /// <summary>Le bouton "Se connecter" reste désactivé tant que les champs sont incomplets.</summary>
    private bool CanLogin() => !IsBusy && Validators.IsEmail(Email) && Password.Length > 0;

    [RelayCommand(CanExecute = nameof(CanLogin))]
    private async Task LoginAsync()
    {
        IsBusy = true;
        ErrorMessage = string.Empty;

        var result = await _auth.LoginAsync(Email, Password);
        IsBusy = false;

        if (!result.Success)
        {
            ErrorMessage = result.Error ?? "Connexion impossible.";
            return;
        }

        Password = string.Empty;
        await Shell.Current.DisplayAlert("Connexion réussie", "Le carnet sera disponible à l'étape suivante.", "OK");
    }

    [RelayCommand]
    private Task GoToRegisterAsync() => Shell.Current.GoToAsync("register");

    [RelayCommand]
    private Task ForgotPasswordAsync() =>
        Shell.Current.DisplayAlert("Mot de passe oublié",
            "La réinitialisation par e-mail nécessite un serveur. Elle n'est pas incluse dans cette version locale.",
            "OK");
}
