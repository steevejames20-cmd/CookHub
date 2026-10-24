namespace CookHub.Services;

/// <summary>
/// Thème clair ou sombre. Un thème sombre réduit la consommation des écrans OLED
/// (choix d'écoconception annoncé dans la note de conception).
///
/// Principe : les écrans n'utilisent que des ressources dynamiques (DynamicResource).
/// Changer de thème revient à remplacer les valeurs de ces ressources : toute l'interface se met à jour.
/// Les valeurs "clair" doivent rester identiques à celles de Resources/Styles/Colors.xaml.
/// </summary>
public static class ThemeService
{
    private const string PreferenceKey = "theme_dark";

    private static readonly Dictionary<string, Color> Light = new()
    {
        ["Bg"] = Color.FromArgb("#FAF7F0"),
        ["Primary"] = Color.FromArgb("#3F6B46"),
        ["Ink"] = Color.FromArgb("#26382C"),
        ["Sage"] = Color.FromArgb("#E8EEDF"),
        ["Error"] = Color.FromArgb("#B54335"),
        ["Surface"] = Color.FromArgb("#FFFFFF"),
        ["Line"] = Color.FromArgb("#D9D6CB"),
        ["Muted"] = Color.FromArgb("#6B7A6F"),
        ["OnPrimary"] = Color.FromArgb("#FFFFFF"),
        ["Disabled"] = Color.FromArgb("#D6D8CC"),
        ["DisabledText"] = Color.FromArgb("#8C9188"),
        ["ErrorBg"] = Color.FromArgb("#F6E3E0"),
        ["Skeleton"] = Color.FromArgb("#E9E6DD"),
    };

    private static readonly Dictionary<string, Color> Dark = new()
    {
        ["Bg"] = Color.FromArgb("#161D18"),
        ["Primary"] = Color.FromArgb("#7DB587"),
        ["Ink"] = Color.FromArgb("#F1EEE4"),
        ["Sage"] = Color.FromArgb("#2A3A2E"),
        ["Error"] = Color.FromArgb("#E58A7E"),
        ["Surface"] = Color.FromArgb("#202A23"),
        ["Line"] = Color.FromArgb("#3A4A3E"),
        ["Muted"] = Color.FromArgb("#A5B2A8"),
        ["OnPrimary"] = Color.FromArgb("#10200F"),
        ["Disabled"] = Color.FromArgb("#37423A"),
        ["DisabledText"] = Color.FromArgb("#7B857E"),
        ["ErrorBg"] = Color.FromArgb("#3D2421"),
        ["Skeleton"] = Color.FromArgb("#2B372E"),
    };

    // Pinceaux dérivés des couleurs : nom du pinceau -> nom de la couleur.
    private static readonly Dictionary<string, string> Brushes = new()
    {
        ["PrimaryBrush"] = "Primary",
        ["InkBrush"] = "Ink",
        ["ErrorBrush"] = "Error",
        ["LineBrush"] = "Line",
        ["MutedBrush"] = "Muted",
    };

    public static bool IsDark => Preferences.Default.Get(PreferenceKey, false);

    /// <summary>À appeler au démarrage : restaure le thème choisi la dernière fois.</summary>
    public static void ApplySaved() => Apply(IsDark);

    /// <summary>Change de thème et mémorise le choix.</summary>
    public static void SetDark(bool dark)
    {
        Preferences.Default.Set(PreferenceKey, dark);
        Apply(dark);
    }

    private static void Apply(bool dark)
    {
        var app = Application.Current;
        if (app is null)
        {
            return;
        }

        var palette = dark ? Dark : Light;
        foreach (var (key, color) in palette)
        {
            app.Resources[key] = color;
        }

        foreach (var (brushKey, colorKey) in Brushes)
        {
            app.Resources[brushKey] = new SolidColorBrush(palette[colorKey]);
        }

        // Aligne aussi les éléments système (barre d'état, clavier, sélecteurs).
        app.UserAppTheme = dark ? AppTheme.Dark : AppTheme.Light;
    }
}
