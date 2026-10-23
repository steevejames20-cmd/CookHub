using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CookHub.Models;

namespace CookHub.ViewModels;

/// <summary>Ligne d'ingrédient affichée dans le détail d'une recette.</summary>
public partial class IngredientItem : ObservableObject
{
    private static readonly CultureInfo French = new("fr-FR");

    private readonly Ingredient _source;

    public IngredientItem(Ingredient source)
    {
        _source = source;
        _quantity = Format(source.Amount, 1);
    }

    public string Name => _source.Name;

    /// <summary>Quantité et unité, par exemple "2 c. à soupe" ou "80 g".</summary>
    [ObservableProperty]
    private string _quantity;

    /// <summary>Case "déjà préparé" cochée par l'utilisateur pendant qu'il cuisine.</summary>
    [ObservableProperty]
    private bool _isChecked;

    /// <summary>Recalcule la quantité pour un autre nombre de personnes (factor = nouvelles / anciennes portions).</summary>
    public void Scale(double factor) => Quantity = Format(_source.Amount, factor);

    private string Format(double amount, double factor)
    {
        if (amount <= 0)
        {
            return _source.Unit;
        }

        // "0.##" : jusqu'à deux décimales, sans zéros inutiles (1,5 et non 1,50).
        var value = (amount * factor).ToString("0.##", French);
        return string.IsNullOrEmpty(_source.Unit) ? value : $"{value} {_source.Unit}";
    }
}
