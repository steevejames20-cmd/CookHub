# Captures d'écran et PDF

Le PDF demandé contient les captures des interfaces en mode simulation (émulateur Android).

## 1. Capturer les 12 interfaces

Application lancée dans l'émulateur, pour chaque écran :

```
adb exec-out screencap -p > captures/01_inscription.png
```

Noms de fichiers attendus (l'ordre du PDF suit ces préfixes) :

| Fichier | Écran | Comment l'obtenir |
|---|---|---|
| 01_inscription.png | Inscription | Connexion, puis "S'inscrire" |
| 02_connexion.png | Connexion | Au lancement, session fermée |
| 03_carnet.png | Mon carnet | Connexion avec le compte de démonstration |
| 04_recherche.png | Recherche | Onglet Recherche, saisir "tomates", filtre Plats |
| 05_detail.png | Détail | Toucher une recette |
| 06_ajout.png | Ajout | Onglet Ajouter, formulaire rempli |
| 07_categories.png | Catégories | Onglet Catégories |
| 08_partage.png | Partage | Détail, bouton Partager |
| 09_carnet_vide.png | Carnet vide | Créer un nouveau compte |
| 10_chargement.png | Chargement | Capture rapide à l'ouverture de l'onglet Carnet |
| 11_recherche_sans_resultat.png | Recherche sans résultat | Rechercher "risotto" |
| 12_ajout_erreurs.png | Ajout avec erreurs | Enregistrer un formulaire vide |

Captures facultatives des fonctionnalités ajoutées : `13_theme_sombre.png`, `14_portions.png`.

## 2. Générer le PDF

```
pip install pillow
python tools/screenshots_to_pdf.py captures CookHub_captures.pdf
```

Le script crée une page de titre, puis une page par capture avec sa légende.
