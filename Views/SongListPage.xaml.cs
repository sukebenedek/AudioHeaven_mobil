using AudioHeaven.Classes;
using AudioHeaven.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Web;

namespace AudioHeaven.Views;

public partial class SongListPage : ContentPage, IQueryAttributable
{
    public ObservableCollection<Song> Songs { get; set; } = new();

    public SongListPage()
	{
		InitializeComponent();
        BindingContext = this;
    }
    
    private bool _isBusy = true;
    public bool IsBusy
    {
        get => _isBusy;
        set { _isBusy = value; OnPropertyChanged(); }
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("title", out var titleObj))
        {
            Title = HttpUtility.UrlDecode(titleObj.ToString());
        }

        if (query.TryGetValue("songs", out var songsObj))
        {
            if (songsObj is List<Song> songs)
            {
                Songs.Clear();
                foreach (var song in songs)
                {
                    Songs.Add(song);
                }
            }
        }
    }
}
