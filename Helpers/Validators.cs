using System.Text.RegularExpressions;

namespace CookHub.Helpers;

/// <summary>Règles de validation partagées par les écrans de connexion et d'inscription.</summary>
public static class Validators
{
    private static readonly Regex EmailPattern = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

    public static bool IsEmail(string? value) => !string.IsNullOrWhiteSpace(value) && EmailPattern.IsMatch(value.Trim());

    /// <summary>Règle 1 de la maquette : 8 caractères minimum.</summary>
    public static bool HasMinLength(string? password) => (password ?? string.Empty).Length >= 8;

    /// <summary>Règle 2 de la maquette : au moins une majuscule et une minuscule.</summary>
    public static bool HasUpperAndLower(string? password) =>
        (password ?? string.Empty).Any(char.IsUpper) && (password ?? string.Empty).Any(char.IsLower);

    /// <summary>Règle 3 de la maquette : au moins un chiffre.</summary>
    public static bool HasDigit(string? password) => (password ?? string.Empty).Any(char.IsDigit);

    public static bool IsStrongPassword(string? password) =>
        HasMinLength(password) && HasUpperAndLower(password) && HasDigit(password);
}
