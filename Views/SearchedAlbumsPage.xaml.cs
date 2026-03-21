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
        Title = $"Albums containing: {UserData.SearchTerm}";
    }
}