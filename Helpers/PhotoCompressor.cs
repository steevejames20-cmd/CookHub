using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Platform;

namespace CookHub.Helpers;

/// <summary>
/// Réduit les photos avant de les enregistrer (écoconception) :
/// 1024 pixels au maximum sur le grand côté, JPEG qualité 80.
/// Une photo de 4 Mo prend ainsi quelques dizaines de Ko.
/// </summary>
public static class PhotoCompressor
{
    private const float MaxSide = 1024f;
    private const float JpegQuality = 0.8f;

    /// <summary>Enregistre la photo choisie, compressée, dans le dossier privé de l'application et retourne son chemin.</summary>
    public static async Task<string> SaveCompressedAsync(FileResult source)
    {
        var folder = Path.Combine(FileSystem.AppDataDirectory, "photos");
        Directory.CreateDirectory(folder);
        var target = Path.Combine(folder, $"{Guid.NewGuid():N}.jpg");

        await using var input = await source.OpenReadAsync();
        try
        {
            using var original = PlatformImage.FromStream(input);
            using var resized = original.Downsize(MaxSide, false);
            await using var output = File.Create(target);
            await resized.SaveAsync(output, ImageFormat.Jpeg, JpegQuality);
        }
        catch (Exception)
        {
            // Repli : si la compression échoue, on copie la photo telle quelle plutôt que de la perdre.
            if (input.CanSeek)
            {
                input.Position = 0;
            }

            await using var output = File.Create(target);
            await input.CopyToAsync(output);
        }

        return target;
    }
}
