using AudioHeaven.Models;
using CommunityToolkit.Mvvm.Messaging;

namespace AudioHeaven.Components;

public partial class PlayerSheet : ContentView
{
    public PlayerSheet()
    {
        InitializeComponent();
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();

        // Unregister first to avoid double-subscriptions if the handler reloads
        WeakReferenceMessenger.Default.Unregister<OpenPlayerMessage>(this);

        // Register the listener
        WeakReferenceMessenger.Default.Register<OpenPlayerMessage>(this, (r, m) =>
        {
            MainThread.BeginInvokeOnMainThread(() => {
                PlayerSheetBottomSheet.IsOpen = true;
            });
        });
    }

    public void Open()
    {
        MainThread.BeginInvokeOnMainThread(() => {
            PlayerSheetBottomSheet.IsOpen = true;
        });
    }

    public void Close()
    {
        PlayerSheetBottomSheet.IsOpen = false;
    }
}