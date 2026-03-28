using AudioHeaven.Classes;
using AudioHeaven.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AudioHeaven.Views;

[QueryProperty(nameof(PlaylistIdString), "id")]
public partial class PlaylistPage : ContentPage
{
    public Playlist playlist { get; set; } = new();
    public ObservableCollection<Song> Songs { get; set; } = new();
    public ICommand RemoveSongCommand { get; }

    public PlaylistPage()
    {
        InitializeComponent();
        RemoveSongCommand = new Command<Song>(async (song) => await RemoveSong(song));
        BindingContext = this;
    }

    private int? _playlistId;

    public string PlaylistIdString
    {
        set
        {
            if (int.TryParse(value, out int id))
                _playlistId = id;
        }
    }

    private bool _isBusy = true;
    public bool IsBusy
    {
        get => _isBusy;
        set { _isBusy = value; OnPropertyChanged(); }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        IsBusy = true;

        try
        {
            int id = _playlistId ?? 0;

            playlist = await API.GetPlaylistByIdAsync(id);

            if (playlist != null)
            {
                BindingContext = playlist;

                if (playlist.Songs != null)
                {
                    Songs.Clear();
                    foreach (var s in playlist.Songs)
                        Songs.Add(s);
                }

                Title = playlist.Title;
            }
            else
            {
                Title = "Playlist Page";
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async void GoToUserPage(object sender, EventArgs e)
    {
        if (BindingContext is Playlist p && p.User != null)
        {
            await Shell.Current.GoToAsync($"UserPage?id={p.User.Id}");
        }
    }

    private void OnPlayClicked(object sender, EventArgs e)
    {
    }

    private async Task RemoveSong(Song swipedSong)
    {
        if (swipedSong == null) return;

        bool confirm = await DisplayAlert("Remove", $"Remove '{swipedSong.Title}'?", "Yes", "Cancel");
        if (confirm)
        {
            await API.DeleteSongFromPlaylistAsync(playlist.Id, swipedSong.Id);
            WeakReferenceMessenger.Default.Send(new RelodadPlaylistsMessage(playlist.Id));
            Songs.Remove(swipedSong);
        }
    }
}