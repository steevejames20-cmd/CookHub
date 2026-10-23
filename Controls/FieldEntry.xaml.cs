namespace CookHub.Controls;

/// <summary>
/// Champ de saisie réutilisable. Exemple :
/// &lt;controls:FieldEntry Caption="Prénom" Text="{Binding FirstName}" IsValid="{Binding FirstNameValid}" /&gt;
/// </summary>
public partial class FieldEntry : ContentView
{
    // Propriétés liables : elles permettent d'utiliser le contrôle avec des liaisons de données.

    public static readonly BindableProperty CaptionProperty = BindableProperty.Create(
        nameof(Caption), typeof(string), typeof(FieldEntry), string.Empty,
        propertyChanged: (b, _, n) => ((FieldEntry)b).CaptionLabel.Text = n as string);

    public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
        nameof(Placeholder), typeof(string), typeof(FieldEntry), string.Empty,
        propertyChanged: (b, _, n) => ((FieldEntry)b).Input.Placeholder = n as string);

    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text), typeof(string), typeof(FieldEntry), string.Empty,
        defaultBindingMode: BindingMode.TwoWay, propertyChanged: OnTextChanged);

    public static readonly BindableProperty IsPasswordProperty = BindableProperty.Create(
        nameof(IsPassword), typeof(bool), typeof(FieldEntry), false,
        propertyChanged: (b, _, n) => ((FieldEntry)b).ApplyPasswordMode((bool)n));

    public static readonly BindableProperty ShowToggleProperty = BindableProperty.Create(
        nameof(ShowToggle), typeof(bool), typeof(FieldEntry), false,
        propertyChanged: (b, _, _) => ((FieldEntry)b).RefreshIcons());

    public static readonly BindableProperty KeyboardTypeProperty = BindableProperty.Create(
        nameof(KeyboardType), typeof(Keyboard), typeof(FieldEntry), Keyboard.Default,
        propertyChanged: (b, _, n) => ((FieldEntry)b).Input.Keyboard = (Keyboard)n);

    public static readonly BindableProperty HelpProperty = BindableProperty.Create(
        nameof(Help), typeof(string), typeof(FieldEntry), string.Empty,
        propertyChanged: (b, _, _) => ((FieldEntry)b).RefreshHelp());

    public static readonly BindableProperty ErrorProperty = BindableProperty.Create(
        nameof(Error), typeof(string), typeof(FieldEntry), string.Empty,
        propertyChanged: (b, _, _) => ((FieldEntry)b).RefreshState());

    public static readonly BindableProperty IsValidProperty = BindableProperty.Create(
        nameof(IsValid), typeof(bool), typeof(FieldEntry), false,
        propertyChanged: (b, _, _) => ((FieldEntry)b).RefreshIcons());

    private bool _isFocused;
    private bool _passwordHidden;

    public FieldEntry()
    {
        InitializeComponent();
        RefreshState();
    }

    public string Caption { get => (string)GetValue(CaptionProperty); set => SetValue(CaptionProperty, value); }
    public string Placeholder { get => (string)GetValue(PlaceholderProperty); set => SetValue(PlaceholderProperty, value); }
    public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }
    public bool IsPassword { get => (bool)GetValue(IsPasswordProperty); set => SetValue(IsPasswordProperty, value); }
    public bool ShowToggle { get => (bool)GetValue(ShowToggleProperty); set => SetValue(ShowToggleProperty, value); }
    public Keyboard KeyboardType { get => (Keyboard)GetValue(KeyboardTypeProperty); set => SetValue(KeyboardTypeProperty, value); }
    public string Help { get => (string)GetValue(HelpProperty); set => SetValue(HelpProperty, value); }
    public string Error { get => (string)GetValue(ErrorProperty); set => SetValue(ErrorProperty, value); }
    public bool IsValid { get => (bool)GetValue(IsValidProperty); set => SetValue(IsValidProperty, value); }

    // Synchronisation entre la propriété Text du contrôle et l'Entry interne.

    private static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var field = (FieldEntry)bindable;
        var text = newValue as string ?? string.Empty;
        if (field.Input.Text != text)
        {
            field.Input.Text = text;
        }
    }

    private void OnInputTextChanged(object? sender, TextChangedEventArgs e)
    {
        var text = e.NewTextValue ?? string.Empty;
        if (Text != text)
        {
            Text = text;
        }
    }

    // Etat ACTIF : la bordure passe au vert et s'épaissit pendant la saisie.

    private void OnInputFocused(object? sender, FocusEventArgs e)
    {
        _isFocused = true;
        RefreshState();
    }

    private void OnInputUnfocused(object? sender, FocusEventArgs e)
    {
        _isFocused = false;
        RefreshState();
    }

    private void OnToggleTapped(object? sender, TappedEventArgs e)
    {
        _passwordHidden = !_passwordHidden;
        Input.IsPassword = _passwordHidden;
        RefreshIcons();
    }

    private void ApplyPasswordMode(bool isPassword)
    {
        _passwordHidden = isPassword;
        Input.IsPassword = isPassword;
        RefreshIcons();
    }

    // Mise à jour de l'apparence selon l'état.

    private void RefreshState()
    {
        var hasError = !string.IsNullOrWhiteSpace(Error);

        if (hasError)
        {
            Box.SetDynamicResource(Border.StrokeProperty, "ErrorBrush");
            Box.StrokeThickness = 1.5;
        }
        else if (_isFocused)
        {
            Box.SetDynamicResource(Border.StrokeProperty, "PrimaryBrush");
            Box.StrokeThickness = 2;
        }
        else
        {
            Box.SetDynamicResource(Border.StrokeProperty, "LineBrush");
            Box.StrokeThickness = 1;
        }

        RefreshHelp();
    }

    private void RefreshHelp()
    {
        var hasError = !string.IsNullOrWhiteSpace(Error);
        HelpLabel.Text = hasError ? Error : Help;
        HelpLabel.IsVisible = !string.IsNullOrWhiteSpace(HelpLabel.Text);
        HelpLabel.SetDynamicResource(Label.TextColorProperty, hasError ? "Error" : "Muted");
    }

    private void RefreshIcons()
    {
        ToggleIcon.IsVisible = ShowToggle;
        ToggleIcon.Icon = _passwordHidden ? "eye" : "eye_off";
        ValidIcon.IsVisible = IsValid && !ShowToggle;
    }
}
