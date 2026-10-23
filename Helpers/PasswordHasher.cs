using System.Security.Cryptography;

namespace CookHub.Helpers;

/// <summary>Calcule et vérifie les empreintes de mots de passe avec PBKDF2.</summary>
public static class PasswordHasher
{
    private const int Iterations = 100_000;
    private const int HashSize = 32;
    private const int SaltSize = 16;

    /// <summary>Génère un sel aléatoire (Base64).</summary>
    public static string NewSalt() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(SaltSize));

    /// <summary>Calcule l'empreinte d'un mot de passe avec le sel donné (Base64).</summary>
    public static string Hash(string password, string salt)
    {
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, Convert.FromBase64String(salt), Iterations,
            HashAlgorithmName.SHA256, HashSize);
        return Convert.ToBase64String(hash);
    }

    /// <summary>Compare en temps constant l'empreinte attendue et celle du mot de passe saisi.</summary>
    public static bool Verify(string password, string salt, string expectedHash)
    {
        var actual = Convert.FromBase64String(Hash(password, salt));
        var expected = Convert.FromBase64String(expectedHash);
        return CryptographicOperations.FixedTimeEquals(actual, expected);
    }
}
