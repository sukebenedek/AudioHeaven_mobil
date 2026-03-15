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
            //await Shell.Current.DisplayAlert("Error", result.Count().ToString(), "Ok");
            if (result != null)
            {
                Songs.Clear();
                foreach (var s in result)
                {
                    //await Shell.Current.DisplayAlert(song.Title, UserData.SearchTerm, "Ok");
                    Songs.Add(s);
                }
            }
        }
        catch (Exception ex)
        {
            //await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
        }
    }
}