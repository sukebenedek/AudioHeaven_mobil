using AudioHeaven.Classes;
using AudioHeaven.Models;
using AudioHeaven.Services;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace AudioHeaven.Components;

public partial class PlayerSheet : ContentView
{
    private readonly MusicService _musicService;

    public PlayerSheet()
    {
        InitializeComponent();
        _musicService = Handler?.MauiContext?.Services.GetService<MusicService>()
                    ?? IPlatformApplication.Current.Services.GetService<MusicService>();
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

    public async void Open()
    {
        MainThread.BeginInvokeOnMainThread(() => {
            PlayerSheetBottomSheet.IsOpen = true;
        });

    }

    public void Close()
    {
        PlayerSheetBottomSheet.IsOpen = false;
    }

    private void TogglePlay(object sender, EventArgs e)
    {
        _musicService.Toggle();
    }

    private async void Skip(object sender, EventArgs e)
    {
        await _musicService.Skip();
    }

    private async void NextSongs(object sender, TappedEventArgs e)
    {
        //var title = "Queue";
        ////var songs = (await API.GetQueueSongsAsync()).ToList();
        //await _musicService.UpdateQueue();
        //var navParam = new Dictionary<string, object>
        //{
        //    { "songs", _musicService.Queue },
        //    { "title", title }
        //};

        Close();
        //await Shell.Current.GoToAsync($"SongListPage", navParam);
        await Shell.Current.GoToAsync($"QueuePage");
    }
}
