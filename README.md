# CookHub

Carnet de recettes consultable et partageable, application mobile .NET MAUI (Android).
Projet de l'atelier de développement mobile, Niv 5 Génie Logiciel, ENSPD, 2026/2027.

Auteur : Bessawa Bone Steeve-James

La maquette (Figma, 12 interfaces) est la référence visuelle : palette crème et potager,
titres Lora, texte Inter, quatre états d'interface (normal, actif, désactivé, erreur).

## Fonctionnalités

| Interface de la maquette | Fichiers principaux |
|---|---|
| 1. Connexion | `Views/LoginPage`, `ViewModels/LoginViewModel` |
| 2. Inscription (validation en direct) | `Views/RegisterPage`, `ViewModels/RegisterViewModel` |
| 3. Mon carnet de recettes | `Views/CarnetPage`, `ViewModels/CarnetViewModel` |
| 4. Recherche et filtres | `Views/SearchPage`, `ViewModels/SearchViewModel` |
| 5. Détail de recette | `Views/DetailPage`, `ViewModels/DetailViewModel` |
| 6. Ajout de recette | `Views/AddRecipePage`, `ViewModels/AddRecipeViewModel` |
| 7. Catégories | `Views/CategoriesPage`, `ViewModels/CategoriesViewModel` |
| 8. Partage de recette | `Views/SharePage`, `ViewModels/ShareViewModel` |
| 9. État : carnet vide | `CarnetPage` (ShowEmpty) |
| 10. État : chargement | `CarnetPage` (ShowLoading, squelette gris) |
| 11. État : recherche sans résultat | `SearchPage` (ShowNoResult) |
| 12. État : ajout avec erreurs | `AddRecipePage` (bandeau et messages d'erreur) |

Fonctionnalités ajoutées au-delà de la maquette :

- thème sombre (économie d'énergie sur écran OLED), choix mémorisé ;
- suppression d'une recette avec confirmation ;
- photo de recette choisie dans la galerie et compressée (1024 px, JPEG 80) ;
- nombre de personnes réglable dans le détail, avec recalcul des quantités, et cases à cocher par ingrédient.

## Lancer l'application

Prérequis : Visual Studio 2022 (charge de travail "Développement d'applications mobiles .NET")
ou le SDK .NET 8 avec la charge de travail MAUI, et un émulateur Android.

```
dotnet workload install maui
dotnet build -t:Run -f net8.0-android
```

Compte de démonstration créé au premier lancement (avec trois recettes) :

- e-mail : `camille.martin@email.fr`
- mot de passe : `Camille2026`

On peut aussi créer un nouveau compte : le carnet est alors vide (interface 9).

## Architecture

Modèle MVVM : chaque écran (`Views`) est lié à un ViewModel (`ViewModels`) qui contient toute la logique.
Les attributs de `CommunityToolkit.Mvvm` génèrent les propriétés et les commandes.

```
Models/        données (Recipe, Ingredient, AppUser...)
Services/      accès aux données : DataStore (fichier JSON), AuthService, RecipeRepository, ThemeService
ViewModels/    logique des écrans
Views/         écrans XAML
Controls/      composants réutilisables : FieldEntry, CategoryChips, RecipeCard, IconView
Converters/    convertisseurs de valeurs pour le XAML
Helpers/       validation, mot de passe, recherche, analyse des ingrédients, compression des photos
Resources/     charte graphique (couleurs, styles), icônes, image de lancement
```

Navigation : un `Shell` avec un écran de connexion, puis une barre de 4 onglets (Carnet, Recherche,
Ajouter, Catégories). Détail, partage et inscription s'ouvrent par-dessus l'onglet courant.

## Écoconception

- données stockées localement : aucune requête réseau pour consulter son carnet ;
- icônes vectorielles dessinées en code (aucun fichier d'image à charger) ;
- photos réduites à 1024 px et compressées avant enregistrement, une seule par recette ;
- squelette gris pendant le chargement, aucun décor à télécharger ;
- fonds unis, sans dégradé ni animation décorative ; thème sombre pour les écrans OLED ;
- partage par lien : seule la recette choisie est envoyée.

## Polices

La charte demande Lora (titres) et Inter (texte). Pour les activer, télécharger les fichiers
`Lora-Regular.ttf` et `Inter-Regular.ttf` (Google Fonts) et les copier dans `Resources/Fonts/`.
Le projet les prend en compte automatiquement. Sans ces fichiers, la police du système est utilisée.

## Historique du dépôt

L'historique de commits suit les phases du devoir :

| Phase | Contenu |
|---|---|
| A | Socle : projet, charte graphique, modèles et services, composants réutilisables |
| B | Authentification, carnet (avec états chargement et vide), recherche (avec état sans résultat) |
| C | Détail, ajout (avec erreurs), catégories, partage |
| Fonctionnalités ajoutées | Thème sombre, suppression, photo, portions et ingrédients cochables |
| Documentation | README, script de la vidéo, outil de génération du PDF de captures |
