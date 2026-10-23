using CookHub.Services;
using CookHub.Views;

namespace CookHub;

/// <summary>
/// Structure de navigation de l'application (voir l'arborescence de la maquette).
/// </summary>
public partial class AppShell : Shell
{
    public AppShell(IAuthService auth)
    {
        InitializeComponent();

        // Écrans secondaires : ouverts par-dessus l'onglet courant,
        // le retour se fait par la flèche en haut à gauche.
        Routing.RegisterRoute("register", typeof(RegisterPage));
        Routing.RegisterRoute("recipe", typeof(DetailPage));

        // Session déjà ouverte : on saute l'écran de connexion.
        if (auth.IsLoggedIn)
        {
            CurrentItem = MainTabs;
        }
    }
}
