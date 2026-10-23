
namespace CookHub;

/// <summary>
/// Classe racine de l'application : crée la fenêtre principale (le Shell).
/// </summary>
public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();
    }
}
