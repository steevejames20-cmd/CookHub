using CookHub.Helpers;
using CookHub.Models;

namespace CookHub.Services;

/// <summary>
/// Données de démonstration créées au premier lancement.
/// Compte : camille.martin@email.fr  /  mot de passe : Camille2026
/// Elles correspondent aux écrans de la maquette Figma.
/// </summary>
public static class SeedData
{
    public const string DemoEmail = "camille.martin@email.fr";
    public const string DemoPassword = "Camille2026";

    public static AppData Create()
    {
        var salt = PasswordHasher.NewSalt();
        var data = new AppData();

        data.Users.Add(new AppUser
        {
            FirstName = "Camille",
            Email = DemoEmail,
            Salt = salt,
            PasswordHash = PasswordHasher.Hash(DemoPassword, salt),
        });

        data.Recipes.Add(new Recipe
        {
            OwnerEmail = DemoEmail,
            AuthorName = "Camille",
            Name = "Tarte aux tomates",
            Category = "Plats",
            Description = "La tarte des beaux jours : croustillante, généreuse et parfumée au basilic.",
            Difficulty = "Facile",
            PrepMinutes = 15,
            CookMinutes = 30,
            Servings = 4,
            AddedOn = new DateTime(2026, 10, 4, 18, 30, 0),
            Ingredients =
            {
                new Ingredient { Amount = 1, Name = "pâte feuilletée" },
                new Ingredient { Amount = 4, Name = "tomates mûres" },
                new Ingredient { Amount = 2, Unit = "c. à soupe", Name = "moutarde" },
                new Ingredient { Amount = 80, Unit = "g", Name = "comté râpé" },
            },
            Steps =
            {
                "Préchauffer le four à 200 degrés et dérouler la pâte dans un moule.",
                "Étaler la moutarde sur le fond de tarte puis parsemer de comté.",
                "Disposer les tomates en rondelles et ajouter le basilic.",
                "Enfourner 30 minutes jusqu'à ce que la pâte soit dorée.",
            },
        });

        data.Recipes.Add(new Recipe
        {
            OwnerEmail = DemoEmail,
            AuthorName = "Camille",
            Name = "Salade de chèvre chaud",
            Category = "Entrées",
            Description = "Une entrée fraîche et gourmande, prête en quelques minutes.",
            Difficulty = "Facile",
            PrepMinutes = 10,
            CookMinutes = 5,
            Servings = 2,
            AddedOn = new DateTime(2026, 10, 2, 12, 0, 0),
            Ingredients =
            {
                new Ingredient { Amount = 1, Name = "bûche de chèvre" },
                new Ingredient { Amount = 4, Name = "tranches de pain" },
                new Ingredient { Amount = 100, Unit = "g", Name = "mesclun" },
                new Ingredient { Amount = 1, Unit = "c. à soupe", Name = "miel" },
            },
            Steps =
            {
                "Couper la bûche de chèvre en rondelles et les poser sur le pain.",
                "Faire gratiner au four quelques minutes.",
                "Dresser sur le mesclun et napper de miel.",
            },
        });

        data.Recipes.Add(new Recipe
        {
            OwnerEmail = DemoEmail,
            AuthorName = "Camille",
            Name = "Gâteau au yaourt",
            Category = "Desserts",
            Description = "Le gâteau de tous les goûters, avec un pot de yaourt pour seule mesure.",
            Difficulty = "Facile",
            PrepMinutes = 10,
            CookMinutes = 35,
            Servings = 6,
            AddedOn = new DateTime(2026, 9, 28, 16, 0, 0),
            Ingredients =
            {
                new Ingredient { Amount = 1, Name = "pot de yaourt nature" },
                new Ingredient { Amount = 2, Name = "pots de sucre" },
                new Ingredient { Amount = 3, Name = "pots de farine" },
                new Ingredient { Amount = 3, Name = "oeufs" },
            },
            Steps =
            {
                "Mélanger le yaourt, le sucre et les oeufs.",
                "Ajouter la farine puis verser dans un moule beurré.",
                "Cuire 35 minutes à 180 degrés.",
            },
        });

        return data;
    }
}
