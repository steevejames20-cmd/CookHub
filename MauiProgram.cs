using CookHub.Services;
using CookHub.ViewModels;
using CookHub.Views;
using Microsoft.Extensions.Logging;

namespace CookHub;

/// <summary>
/// Point d'entrée de l'application : configuration de MAUI et
/// enregistrement des services dans le conteneur d'injection de dépendances.
/// </summary>
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();

        // Services : un seul exemplaire pour toute la durée de vie de l'application.
        builder.Services.AddSingleton<DataStore>();
        builder.Services.AddSingleton<IAuthService, AuthService>();
        builder.Services.AddSingleton<IRecipeRepository, RecipeRepository>();

        // Écrans d'authentification
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<RegisterViewModel>();
        builder.Services.AddTransient<RegisterPage>();

        // Onglet Carnet
        builder.Services.AddTransient<CarnetViewModel>();
        builder.Services.AddTransient<CarnetPage>();

        // Onglet Recherche
        builder.Services.AddTransient<SearchViewModel>();
        builder.Services.AddTransient<SearchPage>();

        // Détail d'une recette
        builder.Services.AddTransient<DetailViewModel>();
        builder.Services.AddTransient<DetailPage>();

        // Onglet Ajouter
        builder.Services.AddTransient<AddRecipeViewModel>();
        builder.Services.AddTransient<AddRecipePage>();

        // Onglet Catégories
        builder.Services.AddTransient<CategoriesViewModel>();
        builder.Services.AddTransient<CategoriesPage>();

        // Partage d'une recette
        builder.Services.AddTransient<ShareViewModel>();
        builder.Services.AddTransient<SharePage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
