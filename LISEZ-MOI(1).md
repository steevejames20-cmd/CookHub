# CookHub : un zip par commit

Chaque zip correspond à UN commit. Procédure, à répéter pour chaque zip dans l'ordre (01, 02, 03...) :

1. Extraire le zip à la racine du dossier du projet (celui qui contient `.git`), en acceptant le remplacement des fichiers existants.
2. Supprimer à la main les fichiers indiqués dans la colonne "À supprimer", s'il y en a.
3. Committer avec le message indiqué.

Au tout début (zip 01) : créer un dossier `CookHub`, y lancer `git init -b main`, puis extraire le zip 01 dedans.

Commandes pour chaque commit (depuis le dossier du projet) :

```
git add -A
git commit -m "<message de la colonne Message>"
```

Faites chaque commit à un moment différent (une étape, un test dans l'émulateur, une pause, l'étape suivante) pour que les heures de l'historique reflètent le travail réel.

| N° | Zip | Message du commit | À supprimer |
|---|---|---|---|
| 01 | `01_phase-a_structure-projet.zip` | `chore(phase-a): structure initiale du projet .NET MAUI` | - |
| 02 | `02_phase-a_charte-graphique.zip` | `feat(phase-a): charte graphique (couleurs, styles) et icônes vectorielles` | - |
| 03 | `03_phase-a_modeles-services.zip` | `feat(phase-a): modèles de données, services et stockage local` | - |
| 04 | `04_phase-a_composants.zip` | `feat(phase-a): composants réutilisables (champ, puces, carte de recette)` | - |
| 05 | `05_phase-b_connexion-inscription.zip` | `feat(phase-b): écrans de connexion et d'inscription` | `Views/StartPage.xaml`, `Views/StartPage.xaml.cs` |
| 06 | `06_phase-b_carnet.zip` | `feat(phase-b): carnet de recettes avec états chargement et vide` | - |
| 07 | `07_phase-b_recherche.zip` | `feat(phase-b): recherche et filtres avec état sans résultat` | - |
| 08 | `08_phase-c_detail-recette.zip` | `feat(phase-c): détail d'une recette` | - |
| 09 | `09_phase-c_ajout-recette.zip` | `feat(phase-c): ajout de recette avec gestion des erreurs` | - |
| 10 | `10_phase-c_categories.zip` | `feat(phase-c): onglet catégories` | - |
| 11 | `11_phase-c_partage.zip` | `feat(phase-c): partage d'une recette par lien` | - |
| 12 | `12_ajout_theme-sombre.zip` | `feat(ajout): thème sombre` | - |
| 13 | `13_ajout_suppression.zip` | `feat(ajout): suppression d'une recette` | - |
| 14 | `14_ajout_photo.zip` | `feat(ajout): photo de recette compressée` | - |
| 15 | `15_ajout_portions-ingredients.zip` | `feat(ajout): nombre de personnes réglable et ingrédients cochables` | - |
| 16 | `16_documentation.zip` | `docs: README, script vidéo et outil de génération du PDF de captures` | - |

Phases : 01 à 04 = phase A, 05 à 07 = phase B, 08 à 11 = phase C, 12 à 15 = fonctionnalités ajoutées, 16 = documentation.

Publication : après les commits, `git remote add origin https://github.com/<compte>/CookHub.git` puis `git push -u origin main` (dépôt public).

Avant de commencer, vérifiez que le projet compile : ouvrez le dossier `projet-final` du premier zip (CookHub-evolutif.zip) dans Visual Studio.
