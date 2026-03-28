using AudioHeaven.Classes;
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

    private async void ToPlaylist(object sender, EventArgs e)
    {
        if (_musicService.SelectedSong != null)
        {
            //var success = await API.AddToPlaylistAsync(_musicService.SelectedSong.Id, 8);
            var myPlaylists = await API.GetUserPlaylistsAsync();

            if (myPlaylists == null || !myPlaylists.Any())
            {
                await Shell.Current.DisplayAlert("Oops", "You don't have any playlists yet.", "OK");
                return;
            }

            // 2. Extract just the names for the Action Sheet
            string[] playlistNames = myPlaylists.Select(p => p.Title).ToArray();

            // 3. Show the popup!
            string selectedName = await Shell.Current.DisplayActionSheet("Add to Playlist", "Cancel", null, playlistNames);

            // 4. Handle the selection
            if (selectedName != "Cancel" && !string.IsNullOrEmpty(selectedName))
            {
                // Find the original playlist object so we get the ID
                var selectedPlaylist = myPlaylists.First(p => p.Title == selectedName);

                bool success = await API.AddToPlaylistAsync(selectedPlaylist.Id, _musicService.SelectedSong.Id);

                if (!success)
                    await Shell.Current.DisplayAlert("Error", $"An unexpected error occured", "OK");
            }
        }
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

}