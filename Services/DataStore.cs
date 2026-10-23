using System.Text.Json;
using CookHub.Helpers;
using CookHub.Models;

namespace CookHub.Services;

/// <summary>
/// Stockage local : un seul fichier JSON dans le dossier privé de l'application.
/// Choix d'écoconception : pas de serveur, pas de base de données lourde,
/// aucune requête réseau pour consulter son carnet.
/// </summary>
public class DataStore
{
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    private readonly string _filePath = Path.Combine(FileSystem.AppDataDirectory, "cookhub.json");
    private readonly SemaphoreSlim _lock = new(1, 1);
    private AppData? _data;

    /// <summary>Retourne les données en mémoire, en les chargeant (ou en les créant) au premier appel.</summary>
    public async Task<AppData> GetAsync()
    {
        await _lock.WaitAsync();
        try
        {
            if (_data is null)
            {
                if (File.Exists(_filePath))
                {
                    await using var stream = File.OpenRead(_filePath);
                    _data = await JsonSerializer.DeserializeAsync<AppData>(stream) ?? new AppData();
                }
                else
                {
                    // Premier lancement : compte de démonstration et trois recettes.
                    _data = SeedData.Create();
                    await WriteAsync();
                }
            }

            return _data;
        }
        finally
        {
            _lock.Release();
        }
    }

    /// <summary>Enregistre l'état courant sur le disque.</summary>
    public async Task SaveAsync()
    {
        await _lock.WaitAsync();
        try
        {
            await WriteAsync();
        }
        finally
        {
            _lock.Release();
        }
    }

    private async Task WriteAsync()
    {
        await using var stream = File.Create(_filePath);
        await JsonSerializer.SerializeAsync(stream, _data, JsonOptions);
    }
}
