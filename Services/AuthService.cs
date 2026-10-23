using CookHub.Helpers;
using CookHub.Models;

namespace CookHub.Services;

/// <summary>
/// Authentification locale. La session est mémorisée dans les préférences de l'appareil
/// (e-mail et prénom), ce qui permet de la restaurer sans lire le fichier de données.
/// </summary>
public class AuthService : IAuthService
{
    private const string EmailKey = "session_email";
    private const string NameKey = "session_name";

    private readonly DataStore _store;

    public AuthService(DataStore store)
    {
        _store = store;
    }

    public bool IsLoggedIn => !string.IsNullOrEmpty(CurrentEmail);

    public string CurrentEmail => Preferences.Default.Get(EmailKey, string.Empty);

    public string CurrentFirstName => Preferences.Default.Get(NameKey, string.Empty);

    public async Task<AuthResult> LoginAsync(string email, string password)
    {
        var data = await _store.GetAsync();
        var normalized = email.Trim().ToLowerInvariant();
        var user = data.Users.FirstOrDefault(u => u.Email == normalized);

        // Message volontairement identique dans les deux cas : on ne révèle pas si le compte existe.
        if (user is null || !PasswordHasher.Verify(password, user.Salt, user.PasswordHash))
        {
            return new AuthResult(false, "Adresse e-mail ou mot de passe incorrect.");
        }

        StartSession(user);
        return new AuthResult(true);
    }

    public async Task<AuthResult> RegisterAsync(string firstName, string email, string password)
    {
        var data = await _store.GetAsync();
        var normalized = email.Trim().ToLowerInvariant();

        if (data.Users.Any(u => u.Email == normalized))
        {
            return new AuthResult(false, "Un compte existe déjà avec cette adresse e-mail.");
        }

        var salt = PasswordHasher.NewSalt();
        var user = new AppUser
        {
            FirstName = firstName.Trim(),
            Email = normalized,
            Salt = salt,
            PasswordHash = PasswordHasher.Hash(password, salt),
        };

        data.Users.Add(user);
        await _store.SaveAsync();

        // Le compte est créé : l'utilisateur est directement connecté.
        StartSession(user);
        return new AuthResult(true);
    }

    public void Logout()
    {
        Preferences.Default.Remove(EmailKey);
        Preferences.Default.Remove(NameKey);
    }

    private static void StartSession(AppUser user)
    {
        Preferences.Default.Set(EmailKey, user.Email);
        Preferences.Default.Set(NameKey, user.FirstName);
    }
}
