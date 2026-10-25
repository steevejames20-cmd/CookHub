using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookHub.Services;

namespace CookHub.ViewModels;

/// <summary>
/// Écran "Partager" : lien de la recette, copie dans le presse-papiers et envoi à un proche.
/// Seule la recette choisie est partagée, le carnet reste privé.
/// </summary>
public partial class ShareViewModel : ObservableObject, IQueryAttributable
{
    private readonly IRecipeRepository _repository;

    public ShareViewModel(IRecipeRepository repository)
    {
        _repository = repository;
    }

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private string _category = string.Empty;

    [ObservableProperty]
    private string _summary = string.Empty;

    [ObservableProperty]
    private string? _photoPath;

    [ObservableProperty]
    private string _link = string.Empty;

    /// <summary>Confirmation affichée après la copie du lien.</summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasFeedback))]
    private string _feedback = string.Empty;

    public bool HasFeedback => !string.IsNullOrEmpty(Feedback);

    // Texte envoyé aux proches : titre de la recette et lien.
    private string ShareText => $"Je te partage ma recette « {Name} » : https://{Link}";

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("id", out var value) && Guid.TryParse(value?.ToString(), out var id))
        {
            _ = LoadAsync(id);
        }
    }

    private async Task LoadAsync(Guid id)
    {
        var recipe = await _repository.GetAsync(id);
        if (recipe is null)
        {
            return;
        }

        Name = recipe.Name;
        Category = recipe.Category;
        Summary = recipe.Summary;
        PhotoPath = recipe.PhotoPath;
        Link = recipe.ShareLink;
    }

    [RelayCommand]
    private async Task CopyLinkAsync()
    {
        await Clipboard.Default.SetTextAsync($"https://{Link}");
        Feedback = "Lien copié dans le presse-papiers.";
    }

    [RelayCommand]
    private async Task SendMessageAsync()
    {
        try
        {
            if (Sms.Default.IsComposeSupported)
            {
                await Sms.Default.ComposeAsync(new SmsMessage { Body = ShareText });
            }
        }
        catch (FeatureNotSupportedException)
        {
            await ShareOtherAsync();
        }
    }

    [RelayCommand]
    private async Task SendEmailAsync()
    {
        try
        {
            if (Email.Default.IsComposeSupported)
            {
                await Email.Default.ComposeAsync(new EmailMessage
                {
                    Subject = $"Recette : {Name}",
                    Body = ShareText,
                });
            }
        }
        catch (FeatureNotSupportedException)
        {
            await ShareOtherAsync();
        }
    }

    /// <summary>Ouvre la feuille de partage du téléphone (WhatsApp, Telegram, etc.).</summary>
    [RelayCommand]
    private Task ShareOtherAsync() =>
        Share.Default.RequestAsync(new ShareTextRequest { Text = ShareText, Title = "Partager la recette" });
}
