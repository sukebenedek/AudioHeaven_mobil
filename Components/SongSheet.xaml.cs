using AudioHeaven.Classes;
using AudioHeaven.Models;
using AudioHeaven.Services;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.ApplicationModel.DataTransfer;

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

        WeakReferenceMessenger.Default.Unregister<OpenSongSheetMessage>(this);

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

    private async void ToPlaylist(object sender, EventArgs e)
    {
        if (_musicService.SelectedSong == null)
            return;

        var myPlaylists = await API.GetUserPlaylistsAsync();

        if (myPlaylists == null || !myPlaylists.Any())
        {
            await Shell.Current.DisplayAlert("Oops", "You don't have any playlists yet.", "OK");
            return;
        }

        var map = myPlaylists.ToDictionary(
            p => $"{p.Title} ({p.LengthFormatted})",
            p => p
        );

        string selectedKey = await Shell.Current.DisplayActionSheet(
            "Add to Playlist",
            "Cancel",
            null,
            map.Keys.ToArray()
        );

        if (selectedKey == "Cancel" || string.IsNullOrEmpty(selectedKey))
            return;

        var selectedPlaylist = map[selectedKey];

        var success = await API.AddToPlaylistAsync(selectedPlaylist.Id, _musicService.SelectedSong.Id);

        if (!success)
            await Shell.Current.DisplayAlert("Error", "Song already added to playlist!", "OK");

        Close();
    }

    private async void ToQueue(object sender, EventArgs e)
    {
        if (_musicService.SelectedSong != null)
        {
            var success = await API.AddToQueueAsync(_musicService.SelectedSong.Id);
            if (success)
            {
                _musicService.UpdateQueue();
            }
        }
        Close();
    }

    private async void ShareSongAsync(object sender, EventArgs e)
    {
        if (_musicService.SelectedSong == null) return;

        await Share.Default.RequestAsync(new ShareTextRequest
        {
            Title = "Share Song",
            Text = $"Check out '{_musicService.SelectedSong.Title}' by '{_musicService.SelectedSong.User.Name}' on AudioHeaven!",
            Uri = $"https://yourwebsite.com/song/{_musicService.SelectedSong.Id}"
        });
    }
        
}