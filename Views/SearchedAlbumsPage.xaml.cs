using AudioHeaven.Classes;
using AudioHeaven.Models;
using System.Collections.ObjectModel;

namespace AudioHeaven.Views;

public partial class SearchedAlbumsPage : ContentPage
{
    public ObservableCollection<Album> Albums { get; set; } = new();

	public SearchedAlbumsPage()
	{
		InitializeComponent();

        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        this.Title = $"Albums containing: {UserData.SearchTerm}";
        try
        {
            var result = await API.GetAlbumsSearchAsync(UserData.SearchTerm);
            //await Shell.Current.DisplayAlert("Error", result.Count().ToString(), "Ok");
            if (result != null)
            {
                Albums.Clear();
                foreach (var song in result)
                {
                    //await Shell.Current.DisplayAlert(song.Title, UserData.SearchTerm, "Ok");
                    Albums.Add(song);
                }
            }
        }
        catch (Exception ex) {
            //await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
        }
    }
}