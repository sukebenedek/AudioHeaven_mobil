using AudioHeaven.Classes;
using AudioHeaven.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AudioHeaven.Views;

[QueryProperty(nameof(TargetUser), "SelectedUser")]
public partial class UserPage : ContentPage, INotifyPropertyChanged
{
    private Models.User _targetUser;
    public Models.User TargetUser
    {
        get => _targetUser;
        set
        {
            _targetUser = value;
            OnPropertyChanged();
            Title = _targetUser?.Name;
        }
    }

    public ObservableCollection<Song> Songs
    {
        get => _songs;
        set
        {
            _songs = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(DoesNotHaveSongs));
        }
    }
    private ObservableCollection<Song> _songs = new();

    public bool HasSongs
    {
        get => _hasSongs;
        set
        {
            _hasSongs = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(DoesNotHaveSongs));
        }
    }
    private bool _hasSongs;

    public bool DoesNotHaveSongs => !HasSongs;

    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            _isBusy = value;
            OnPropertyChanged();
        }
    }
    private bool _isBusy = true;

    public UserPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadSongsAsync();
    }

    public async Task LoadSongsAsync()
    {
        var songs = await API.GetUserSongsAsync(TargetUser.Id);

        Songs = new ObservableCollection<Song>(
            songs.OrderByDescending(s => s.Plays).Take(5)
        );

        HasSongs = Songs.Any();
        UserData.Songs = Songs.ToList();
        IsBusy = false;
    }

    public new event PropertyChangedEventHandler PropertyChanged;

    protected new void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    private async void OnSongsHeaderClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"AllSongsPage?id={TargetUser.Id}");
    }
}