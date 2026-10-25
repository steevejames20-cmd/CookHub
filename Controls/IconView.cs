namespace CookHub.Controls;

/// <summary>
/// Icône vectorielle, utilisable en XAML :  &lt;controls:IconView Icon="search" /&gt;
/// La couleur par défaut est celle de l'encre de la charte graphique
/// et suit le thème clair ou sombre. Pour une autre couleur, renseigner Fill.
///
/// Le type Path de .NET MAUI est scellé : on ne peut pas en hériter.
/// Le contrôle est donc un ContentView qui contient un Path (composition).
/// Le nom complet du Path est écrit pour éviter le conflit avec System.IO.Path
/// (usings implicites activés dans le projet).
/// </summary>
public class IconView : ContentView
{
    /// <summary>Tracé affiché par le contrôle.</summary>
    private readonly Microsoft.Maui.Controls.Shapes.Path _path = new()
    {
        Aspect = Stretch.Uniform
    };

    public static readonly BindableProperty IconProperty = BindableProperty.Create(
        nameof(Icon), typeof(string), typeof(IconView), string.Empty, propertyChanged: OnIconChanged);

    public static readonly BindableProperty FillProperty = BindableProperty.Create(
        nameof(Fill), typeof(Brush), typeof(IconView), null, propertyChanged: OnFillChanged);

    public IconView()
    {
        WidthRequest = 22;
        HeightRequest = 22;
        HorizontalOptions = LayoutOptions.Center;
        VerticalOptions = LayoutOptions.Center;

        Content = _path;

        // Couleur par défaut : l'encre de la charte (suit le thème).
        _path.SetDynamicResource(Microsoft.Maui.Controls.Shapes.Shape.FillProperty, "InkBrush");
    }

    /// <summary>Nom de l'icône (voir le catalogue Icons).</summary>
    public string Icon
    {
        get => (string)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <summary>Couleur de l'icône. Si elle n'est pas renseignée, la couleur d'encre du thème est utilisée.</summary>
    public Brush? Fill
    {
        get => (Brush?)GetValue(FillProperty);
        set => SetValue(FillProperty, value);
    }

    private static void OnIconChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ((IconView)bindable)._path.Data = Icons.Get(newValue as string);
    }

    private static void OnFillChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var icon = (IconView)bindable;

        if (newValue is Brush brush)
        {
            icon._path.Fill = brush;
        }
        else
        {
            // Retour à la couleur par défaut du thème.
            icon._path.SetDynamicResource(Microsoft.Maui.Controls.Shapes.Shape.FillProperty, "InkBrush");
        }
    }
}
