using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookHub.Helpers;
using CookHub.Services;

namespace CookHub.ViewModels;

/// <summary>
/// Logique de l'écran d'inscription : validation en direct de chaque champ
/// (coche verte, règles du mot de passe) comme dans la maquette.
/// </summary>
public partial class RegisterViewModel : ObservableObject
{
    private readonly IAuthService _auth;

    public RegisterViewModel(IAuthService auth)
    {
        _auth = auth;
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FirstNameValid))]
    [NotifyPropertyChangedFor(nameof(FirstNameHelp))]
    [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    [NotifyPropertyChangedFor(nameof(ShowIncompleteHint))]
    private string _firstName = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(EmailValid))]
    [NotifyPropertyChangedFor(nameof(EmailHelp))]
    [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    [NotifyPropertyChangedFor(nameof(ShowIncompleteHint))]
    private string _email = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasMinLength))]
    [NotifyPropertyChangedFor(nameof(MinLengthMissing))]
    [NotifyPropertyChangedFor(nameof(HasUpperAndLower))]
    [NotifyPropertyChangedFor(nameof(UpperAndLowerMissing))]
    [NotifyPropertyChangedFor(nameof(HasDigit))]
    [NotifyPropertyChangedFor(nameof(DigitMissing))]
    [NotifyPropertyChangedFor(nameof(PasswordsMatch))]
    [NotifyPropertyChangedFor(nameof(ConfirmError))]
    [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    [NotifyPropertyChangedFor(nameof(ShowIncompleteHint))]
    private string _password = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PasswordsMatch))]
    [NotifyPropertyChangedFor(nameof(ConfirmError))]
    [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    [NotifyPropertyChangedFor(nameof(ShowIncompleteHint))]
    private string _confirmPassword = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    [NotifyPropertyChangedFor(nameof(ShowIncompleteHint))]
    private bool _acceptTerms;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasError))]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    private bool _isBusy;

    // Etat de validation de chaque champ

    public bool FirstNameValid => FirstName.Trim().Length > 0;
    public string FirstNameHelp => FirstNameValid ? "Prénom renseigné." : string.Empty;

    public bool EmailValid => Validators.IsEmail(Email);
    public string EmailHelp => EmailValid ? "Le format de l'adresse est valide." : string.Empty;

    // Les trois règles du mot de passe : chaque règle a son état "validée" et son contraire,
    // car le XAML affiche une coche ou un cercle vide selon le cas.
    public bool HasMinLength => Validators.HasMinLength(Password);
    public bool MinLengthMissing => !HasMinLength;
    public bool HasUpperAndLower => Validators.HasUpperAndLower(Password);
    public bool UpperAndLowerMissing => !HasUpperAndLower;
    public bool HasDigit => Validators.HasDigit(Password);
    public bool DigitMissing => !HasDigit;

    public bool PasswordsMatch => ConfirmPassword.Length > 0 && ConfirmPassword == Password;

    /// <summary>Erreur affichée seulement quand l'utilisateur a commencé à saisir la confirmation.</summary>
    public string ConfirmError =>
        ConfirmPassword.Length > 0 && ConfirmPassword != Password
            ? "Les deux mots de passe doivent être identiques."
            : string.Empty;

    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    /// <summary>Texte de la charte pour l'état DESACTIVE du bouton.</summary>
    public bool ShowIncompleteHint => !CanRegister();

    private bool CanRegister() =>
        !IsBusy && FirstNameValid && EmailValid && Validators.IsStrongPassword(Password) &&
        PasswordsMatch && AcceptTerms;

    [RelayCommand(CanExecute = nameof(CanRegister))]
    private async Task RegisterAsync()
    {
        IsBusy = true;
        ErrorMessage = string.Empty;

        var result = await _auth.RegisterAsync(FirstName, Email, Password);
        IsBusy = false;

        if (!result.Success)
        {
            ErrorMessage = result.Error ?? "Inscription impossible.";
            return;
        }

        await Shell.Current.DisplayAlert("Compte créé", "Le carnet sera disponible à l'étape suivante.", "OK");
    }
}
