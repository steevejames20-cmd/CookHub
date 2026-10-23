namespace CookHub.Services;

/// <summary>Résultat d'une tentative de connexion ou d'inscription.</summary>
public record AuthResult(bool Success, string? Error = null);

/// <summary>Gestion des comptes et de la session de l'utilisateur.</summary>
public interface IAuthService
{
    /// <summary>Vrai si un utilisateur est connecté (session mémorisée entre deux lancements).</summary>
    bool IsLoggedIn { get; }

    string CurrentEmail { get; }
    string CurrentFirstName { get; }

    Task<AuthResult> LoginAsync(string email, string password);
    Task<AuthResult> RegisterAsync(string firstName, string email, string password);
    void Logout();
}
