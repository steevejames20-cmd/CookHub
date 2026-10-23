using CookHub.Services;

namespace CookHub;

/// <summary>
/// Classe racine de l'application : crée la fenêtre principale (le Shell).
/// </summary>
public partial class App : Application
{
    public App(IAuthService auth)
    {
        InitializeComponent();
        // Le Shell choisit l'écran de départ selon la présence d'une session ouverte.
        MainPage = new AppShell(auth);
    }
}
