using AudioHeaven.Models;
using AudioHeaven.Services;
using CommunityToolkit.Mvvm.Messaging;

namespace AudioHeaven.Components;

public partial class SongSheet : ContentView
{
    private readonly MusicService _musicService;

    public SongSheet()
	{
		InitializeComponent();

        _musicService = Handler?.MauiContext?.Services.GetService<MusicService>()
                    ?? IPlatformApplication.Current.Services.GetService<MusicService>();
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();

        // Unregister first to avoid double-subscriptions if the handler reloads
        WeakReferenceMessenger.Default.Unregister<OpenSongSheetMessage>(this);

        // Register the listener
        WeakReferenceMessenger.Default.Register<OpenSongSheetMessage>(this, (r, m) =>
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                _musicService.SelectedSong = m.Song; 
                SongSheetBottomSheet.IsOpen = true;
            });
        });
    }

    public async void Open()
    {
        MainThread.BeginInvokeOnMainThread(() => {
            SongSheetBottomSheet.IsOpen = true;
        });

    }

    public void Close()
    {
        SongSheetBottomSheet.IsOpen = false;
    }
}