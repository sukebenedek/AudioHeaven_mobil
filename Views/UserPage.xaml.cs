using AudioHeaven.Classes;
using AudioHeaven.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AudioHeaven.Views;

// Change "SelectedUser" to "UserId" to match the incoming data
[QueryProperty(nameof(UserId), "id")]
public partial class UserPage : ContentPage, INotifyPropertyChanged
{
    private int _userId;
    public int UserId
    {
        get => _userId;
        set
        {
            _userId = value;
            OnPropertyChanged();
            // We don't load here because it might be too fast for the lifecycle
        }
    }

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

    private ObservableCollection<Song> _songs = new();
    public ObservableCollection<Song> Songs
    {
        get => _songs;
        set
        {
            _songs = value;
            OnPropertyChanged(); // CRITICAL: Tells XAML to refresh the list
        }
    }

    private bool _hasSongs;
    public bool HasSongs
    {
        get => _hasSongs;
        set { _hasSongs = value; OnPropertyChanged(); OnPropertyChanged(nameof(DoesNotHaveSongs)); }
    }

    public bool DoesNotHaveSongs => !HasSongs;

    private bool _isBusy = true;
    public bool IsBusy
    {
        get => _isBusy;
        set { _isBusy = value; OnPropertyChanged(); }
    }

    public UserPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadDataAsync();
    }

    public async Task LoadDataAsync()
    {
        IsBusy = true;

        // 1. Fetch the User details first
        var user = await API.GetUserByIdAsync(UserId);
        if (user != null)
        {
            TargetUser = user;

            // 2. Fetch the songs for this specific ID
            var songs = await API.GetUserSongsAsync(UserId);
            if (songs != null)
            {
                Songs = new ObservableCollection<Song>(
                    songs.OrderByDescending(s => s.Plays).Take(5)
                );
                HasSongs = Songs.Any();

            }
        }

        IsBusy = false;
    }

    public new event PropertyChangedEventHandler PropertyChanged;
    protected new void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    private async void OnSongsHeaderClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"AllSongsPage?id={UserId}");
    }
}