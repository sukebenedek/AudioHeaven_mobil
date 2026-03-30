using AudioHeaven.Classes;
using AudioHeaven.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AudioHeaven.Views;

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
            OnPropertyChanged(); 
        }
    }

    private bool _hasSongs;
    public bool HasSongs
    {
        get => _hasSongs;
        set { _hasSongs = value; OnPropertyChanged();}
    }

    private ObservableCollection<Album> _albums = new();
    public ObservableCollection<Album> Albums
    {
        get => _albums;
        set
        {
            _albums = value;
            OnPropertyChanged(); 
        }
    }

    private bool _hasAlbums;
    public bool HasAlbums
    {
        get => _hasAlbums;
        set { _hasAlbums = value; OnPropertyChanged();  }
    }

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

        var user = await API.GetUserByIdAsync(UserId);
        if (user != null)
        {
            TargetUser = user;

            var songs = await API.GetUserSongsAsync(UserId);
            if (songs != null)
            {
                Songs = new ObservableCollection<Song>(
                    songs.OrderByDescending(s => s.Plays).Take(5)
                );
                HasSongs = Songs.Any();

            }

            var albums = await API.GetUserAlbumsAsync(UserId);
            if (albums != null)
            {
                Albums = new ObservableCollection<Album>(
                    albums.OrderByDescending(a => a.CreatedAt).Take(5)
                );
                HasAlbums = Albums.Any();

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

    private async void OnAlbumsHeaderClicked(object sender, EventArgs e)
    {
        var result = await API.GetUserAlbumsAsync(UserId);
        var parameters = new Dictionary<string, object>
        {
            { "albums", result.ToList() } 
        };

        await Shell.Current.GoToAsync("SearchedAlbumsPage", parameters);

    }
}