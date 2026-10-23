namespace CookHub.Models;

/// <summary>Un compte utilisateur. Le mot de passe n'est jamais stocké en clair.</summary>
public class AppUser
{
    public string FirstName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    /// <summary>Empreinte du mot de passe (PBKDF2), encodée en Base64.</summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>Sel aléatoire propre à l'utilisateur, encodé en Base64.</summary>
    public string Salt { get; set; } = string.Empty;
}
