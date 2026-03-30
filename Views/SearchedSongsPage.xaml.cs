using AudioHeaven.Classes;
using AudioHeaven.Models;
using System.Collections.ObjectModel;

namespace AudioHeaven.Views;

public partial class SearchedSongsPage : ContentPage
{
    public ObservableCollection<Song> Songs { get; set; } = new();
    
    public SearchedSongsPage()
    {
        InitializeComponent();

        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        this.Title = $"Songs containing: {UserData.SearchTerm}";
        try
        {
            var result = await API.GetSongsSearchAsync(UserData.SearchTerm);
            if (result != null)
            {
                Songs.Clear();
                foreach (var s in result)
                {
                    Songs.Add(s);
                }
            }
        }
        catch (Exception ex){}
    }
}