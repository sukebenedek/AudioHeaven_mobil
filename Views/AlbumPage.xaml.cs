using System.Collections.ObjectModel;
	using AudioHeaven.Classes;
using AudioHeaven.Models;
using AudioHeaven.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AudioHeaven.Views;

[QueryProperty(nameof(AlbumIdString), "id")]
public partial class AlbumPage : ContentPage
{
    public Album album { get; set; } = new();
    public ObservableCollection<IndexedSong> Songs { get; set; } = new();

    public AlbumPage()
	{
		InitializeComponent();
        BindingContext = this;
	}

    private int? _albumId;

    public string AlbumIdString
    {
        set
        {
            if (int.TryParse(value, out int id))
                _albumId = id;
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
            int id = _albumId ?? 0;

            album = await API.GetAlbumByIdAsync(id);

            if (album != null)
            {
                BindingContext = album; 

                if (album.Songs != null)
                {
                    Songs.Clear();

                    for (int i = 0; i < album.Songs.Count; i++)
                    {
                        Songs.Add(new IndexedSong
                        {
                            Index = i + 1, // 1-based index
                            Song = album.Songs[i]
                        });
                    }
                }

                Title = album.Title;
            }
            else
            {
                Title = "Album Page";
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async void GoToUserPage(object sender, EventArgs e)
    {
        if (BindingContext is Album a && a.User != null)
        {
            //var navParam = new Dictionary<string, User> { { "SelectedUserId", song.User.Id } };
            //await Shell.Current.GoToAsync("UserPage", navParam);
            // Navigation by ID
            await Shell.Current.GoToAsync($"UserPage?id={a.User.Id}");
        }
    }

    private async void OnPlayClicked(object sender, EventArgs e)
    {
        var musicService = IPlatformApplication.Current.Services.GetService<MusicService>();

        await API.DeleteQueueAsync();
        await API.AddManyToQueueAsync(Songs.Select(s => s.Song.Id));

        if (musicService != null)
        {
            await musicService.UpdateQueue(); 
            await musicService.Skip();        
        }
    }
}