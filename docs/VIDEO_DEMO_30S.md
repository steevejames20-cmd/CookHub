# Vidéo de démonstration (30 secondes maximum)

Objectif demandé : montrer l'application en fonctionnement et les fonctionnalités ajoutées.

## Enregistrement

Depuis l'émulateur Android (application lancée) :

```
adb shell screenrecord --time-limit 30 /sdcard/demo.mp4
adb pull /sdcard/demo.mp4
```

Pour enregistrer plus confortablement, l'émulateur propose aussi un bouton d'enregistrement d'écran
dans sa barre latérale (Screen record). Pas de commentaire audio nécessaire.

## Déroulé minute par minute

| Temps | Action à l'écran | Ce que cela montre |
|---|---|---|
| 0 - 4 s | Connexion avec le compte de démonstration | Authentification, bouton actif après saisie |
| 4 - 8 s | Carnet : défiler, toucher la puce "Plats" | Liste, filtres par catégorie |
| 8 - 12 s | Onglet Recherche : taper "tomates" | Recherche par ingrédient |
| 12 - 18 s | Onglet Ajouter : enregistrer sans nom (erreur), puis saisir un nom et une étape et enregistrer | État d'erreur, conservation des saisies, ajout |
| 18 - 23 s | Détail : changer le nombre de personnes, cocher un ingrédient | Fonctionnalité ajoutée : portions et cases à cocher |
| 23 - 27 s | Partager : copier le lien | Partage par lien |
| 27 - 30 s | Menu du compte : passer au thème sombre | Fonctionnalité ajoutée : thème sombre |

Répéter une fois avant d'enregistrer : 30 secondes passent vite.
