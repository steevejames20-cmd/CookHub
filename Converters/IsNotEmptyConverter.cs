using System.Globalization;

namespace CookHub.Converters;

/// <summary>Vrai si le texte n'est ni nul ni vide : affiche un élément seulement s'il y a du contenu.</summary>
public class IsNotEmptyConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        !string.IsNullOrWhiteSpace(value as string);

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
