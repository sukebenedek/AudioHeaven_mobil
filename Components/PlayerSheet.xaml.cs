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

    private async void Back(object sender, EventArgs e)
    {
        await _musicService.Back();
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

    private async void OnGoToSongPageClicked(object sender, EventArgs e)
    {
        if (_musicService.CurrentSong.AlbumId != null)
        {
            await Shell.Current.GoToAsync($"AlbumPage?id={_musicService.CurrentSong.AlbumId}");
            Close();
            return;
        }
        await Shell.Current.GoToAsync($"SongPage?id={_musicService.CurrentSong.Id}");
        Close();

    }

    private async void OnGoToUserPageClicked(object sender, EventArgs e)
    {
        if (_musicService.CurrentSong.User != null)
        {
            //var navParam = new Dictionary<string, User> { { "SelectedUserId", song.User.Id } };
            //await Shell.Current.GoToAsync("UserPage", navParam);
            // Navigation by ID
            await Shell.Current.GoToAsync($"UserPage?id={_musicService.CurrentSong.User.Id}");
            Close();

        }
    }

    private double _pendingSeekValue;

    private void OnSliderChanged(object sender, ValueChangedEventArgs e)
    {
        _pendingSeekValue = e.NewValue;
    }

    private void OnSliderDragStarted(object sender, EventArgs e)
    {
        _musicService?.SetSeeking(true);
    }

    private void OnSliderDragCompleted(object sender, EventArgs e)
    {
        if (_musicService == null) return;

        _musicService.Seek(_pendingSeekValue);
        _musicService.SetSeeking(false);
    }
}
