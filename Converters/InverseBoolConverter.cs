using System.Globalization;

namespace CookHub.Converters;

/// <summary>Inverse un booléen : utile pour afficher un élément quand une condition est fausse.</summary>
public class InverseBoolConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is bool b ? !b : value;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is bool b ? !b : value;
}
