using AudioHeaven.Classes;
using AudioHeaven.Models;
using AudioHeaven.Services;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AudioHeaven.Views;

[QueryProperty(nameof(SongId), "id")]
public partial class SongPage : ContentPage, INotifyPropertyChanged
{
    private int _songId;
    public int SongId
    {
        get => _songId;
        set { _songId = value; OnPropertyChanged(); }
    }

    private Song _currentSong;
    public Song CurrentSong
    {
        get => _currentSong;
        set { _currentSong = value; OnPropertyChanged(); }
    }

    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set { _isBusy = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsNotBusy)); }
    }
    public bool IsNotBusy => !IsBusy;

    public Command GoToArtistCommand { get; }
    public SongPage()
    {
        InitializeComponent();
        BindingContext = this;
        GoToArtistCommand = new Command(async () => await Shell.Current.GoToAsync($"UserPage?id={CurrentSong.UserId}"));
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadSongAsync();
    }

    private async Task LoadSongAsync()
    {
        IsBusy = true;
        var song = await API.GetSongByIdAsync(SongId);
        if (song != null)
        {
            CurrentSong = song;
        }
        IsBusy = false;
    }

    private void OnPlayClicked(object sender, EventArgs e)
    {
        if (CurrentSong == null) return;

        var musicService = IPlatformApplication.Current.Services.GetService<MusicService>();
        musicService?.PlaySong(CurrentSong);
    }

    public new event PropertyChangedEventHandler PropertyChanged;
    protected new void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
    private async void  GoToUserPage(object sender, EventArgs e)
    {
            if (CurrentSong?.User != null)
            {
                await Shell.Current.GoToAsync($"UserPage?id={CurrentSong.User.Id}");
            }
    }
    private async void OnHeaderClicked(object sender, EventArgs e)
    {
        if (CurrentSong?.Album != null && CurrentSong?.Album.Id != 0 && CurrentSong?.Album.Id != null)
        {
            await Shell.Current.GoToAsync($"AlbumPage?id={CurrentSong?.Album.Id}");
        }
    }

    private void OnMeatballsClicked(object sender, EventArgs e)
    {
        var services = IPlatformApplication.Current.Services;

        var musicService = IPlatformApplication.Current.Services.GetService<MusicService>();

        musicService.SelectedSong = CurrentSong;

        WeakReferenceMessenger.Default.Send(new OpenSongSheetMessage(CurrentSong));
    }
}