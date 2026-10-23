using Android.App;
using Android.Runtime;

namespace CookHub;

/// <summary>Classe Application Android : démarre MAUI via MauiProgram.</summary>
[Application]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership) : base(handle, ownership)
    {
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
