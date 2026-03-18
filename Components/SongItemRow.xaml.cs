using AudioHeaven.Models;
using AudioHeaven.Services;

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

    public SongItemRow()
    {
        InitializeComponent();
    }

    static void OnFormatChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (SongItemRow)bindable;

        var format = newValue?.ToString();

        control.ShowPlays = format == "plays";
        control.ShowUsername = format == "username";

        control.OnPropertyChanged(nameof(ShowPlays));
        control.OnPropertyChanged(nameof(ShowUsername));
    }

    // 1. CLICKED THE WHOLE ROW: Play Song
    private void OnPlaySongClicked(object sender, EventArgs e)
    {
        if (BindingContext is Song song)
        {
            var musicService = IPlatformApplication.Current.Services.GetService<MusicService>();

            // Just call the service method!
            musicService?.PlaySong(song);
        }
    }

    // 2. CLICKED TITLE: Go to SongPage
    private async void OnGoToSongPageClicked(object sender, EventArgs e)
    {
        if (BindingContext is Song song)
        {
            var navParam = new Dictionary<string, object> { { "SelectedSong", song } };
            await Shell.Current.GoToAsync("SongPage", navParam);
        }
    }

    // 3. CLICKED USERNAME: Go to UserPage
    private async void OnGoToUserPageClicked(object sender, EventArgs e)
    {
        if (BindingContext is Song song && song.User != null)
        {
            //var navParam = new Dictionary<string, User> { { "SelectedUserId", song.User.Id } };
            //await Shell.Current.GoToAsync("UserPage", navParam);
        }
    }
}