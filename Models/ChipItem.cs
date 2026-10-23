using CommunityToolkit.Mvvm.ComponentModel;

namespace CookHub.Models;

/// <summary>Une puce de filtre (Tout, Plats, Desserts...). IsSelected change son apparence.</summary>
public partial class ChipItem : ObservableObject
{
    public ChipItem(string label, bool isSelected = false)
    {
        Label = label;
        _isSelected = isSelected;
    }

    public string Label { get; }

    [ObservableProperty]
    private bool _isSelected;
}
