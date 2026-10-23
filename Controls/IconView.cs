using Microsoft.Maui.Controls.Shapes;

namespace CookHub.Controls;

/// <summary>
/// Icône vectorielle, utilisable en XAML :  &lt;controls:IconView Icon="search" /&gt;
/// La couleur par défaut est celle de l'encre de la charte graphique
/// et suit le thème clair ou sombre. Pour une autre couleur, renseigner Fill.
/// </summary>
public class IconView : Microsoft.Maui.Controls.Shapes.Path
{
    public static readonly BindableProperty IconProperty = BindableProperty.Create(
        nameof(Icon), typeof(string), typeof(IconView), string.Empty, propertyChanged: OnIconChanged);

    public IconView()
    {
        Aspect = Stretch.Uniform;
        WidthRequest = 22;
        HeightRequest = 22;
        HorizontalOptions = LayoutOptions.Center;
        VerticalOptions = LayoutOptions.Center;

        // Couleur par défaut : l'encre de la charte (suit le thème).
        this.SetDynamicResource(FillProperty, "InkBrush");
    }

    /// <summary>Nom de l'icône (voir le catalogue Icons).</summary>
    public string Icon
    {
        get => (string)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    private static void OnIconChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ((IconView)bindable).Data = Icons.Get(newValue as string);
    }
}
