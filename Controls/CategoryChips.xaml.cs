using System.Collections;
using System.Windows.Input;

namespace CookHub.Controls;

/// <summary>
/// Rangée de puces de filtre. Items : liste de ChipItem ; Command reçoit la puce touchée.
/// </summary>
public partial class CategoryChips : ContentView
{
    public static readonly BindableProperty ItemsProperty = BindableProperty.Create(
        nameof(Items), typeof(IEnumerable), typeof(CategoryChips), null);

    public static readonly BindableProperty CommandProperty = BindableProperty.Create(
        nameof(Command), typeof(ICommand), typeof(CategoryChips), null);

    public CategoryChips()
    {
        InitializeComponent();
    }

    public IEnumerable? Items
    {
        get => (IEnumerable?)GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }
}
