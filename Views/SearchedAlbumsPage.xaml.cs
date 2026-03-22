using AudioHeaven.Classes;
using AudioHeaven.Models;
using System.Collections.ObjectModel;

namespace AudioHeaven.Views;

public partial class SearchedAlbumsPage : ContentPage, IQueryAttributable
{
    public ObservableCollection<Album> Albums { get; set; } = new();

    public SearchedAlbumsPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("albums", out var albumsObj))
        {
            if (albumsObj is List<Album> albums)
            {
                Albums.Clear();

                foreach (var album in albums)
                    Albums.Add(album);
            }
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (Albums.Any(a => a.UserId != Albums[0].UserId))
            Title = $"Albums containing: {UserData.SearchTerm}";
        else
        {
            if(Albums[0] != null && Albums[0].User != null)
                Title = $"{Albums[0].User.Name}'s Albums";
            else
                Title = $"{UserData.User.Name}'s Albums";
        }
    }
}