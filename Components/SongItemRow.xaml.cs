using AudioHeaven.Models;
using AudioHeaven.Services;
using AudioHeaven.ViewModels;
using CommunityToolkit.Mvvm.Messaging;

namespace AudioHeaven.Components;

public partial class SongItemRow : ContentView
{
    public static readonly BindableProperty FormatProperty =
        BindableProperty.Create(
            nameof(Format),
            typeof(string),
            typeof(SongItemRow),
            null,
            propertyChanged: OnFormatChanged);

    public string Format
    {
        get => (string)GetValue(FormatProperty);
        set => SetValue(FormatProperty, value);
    }

    public bool ShowPlays { get; set; }
    public bool ShowUsername { get; set; }
    public bool IsCard { get; set; }

    private readonly MusicService _musicService;
    public SongItemRow()
    {
        InitializeComponent();

        var services = IPlatformApplication.Current.Services;

        _musicService = services.GetService<MusicService>();

    }

    static void OnFormatChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (SongItemRow)bindable;

        var format = newValue?.ToString();

        control.ShowPlays = format == "plays";
        control.ShowUsername = format == "username";
        control.IsCard = format == "card";

        control.OnPropertyChanged(nameof(ShowPlays));
        control.OnPropertyChanged(nameof(ShowUsername));
        control.OnPropertyChanged(nameof(IsCard));
    }

    private void OnPlaySongClicked(object sender, EventArgs e)
    {
        if (BindingContext is Song song)
        {
            var musicService = IPlatformApplication.Current.Services.GetService<MusicService>();

            // Just call the service method!
            musicService?.PlaySong(song);
        }
    }

    private async void OnGoToSongPageClicked(object sender, EventArgs e)
    {
        if (BindingContext is Song song)
        {
            if(song.AlbumId != null)
            {
                await Shell.Current.GoToAsync($"AlbumPage?id={song.AlbumId}");
                return;
            }
            await Shell.Current.GoToAsync($"SongPage?id={song.Id}");
        }
    }

    private async void OnGoToUserPageClicked(object sender, EventArgs e)
    {
        if (BindingContext is Song song && song.User != null)
        {
            //var navParam = new Dictionary<string, User> { { "SelectedUserId", song.User.Id } };
            //await Shell.Current.GoToAsync("UserPage", navParam);
            // Navigation by ID
            await Shell.Current.GoToAsync($"UserPage?id={song.User.Id}");
        }
    }

    private void OnMeatballsClicked(object sender, EventArgs e)
    {
        if (BindingContext is Song song)
        {
            _musicService.SelectedSong = song;

            WeakReferenceMessenger.Default.Send(new OpenSongSheetMessage(song));
        }
    }

    //private void OnOpenRequested()
    //{
    //    if (BindingContext is Song song)
    //            {
    //                WeakReferenceMessenger.Default.Send(new OpenSongSheetMessage());
    //        _musicService.SelectedSong = song;

    //    }
    //}
}