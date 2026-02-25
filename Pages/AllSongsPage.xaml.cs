using AudioHeaven.Classes;
using AudioHeaven.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace AudioHeaven.Pages;

public partial class AllSongsPage : ContentPage
{
    public ObservableCollection<Song> Songs { get; set; } = new();

    public AllSongsPage()
    {
        InitializeComponent();

        BindingContext = this;
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var result = await API.GetUserSongsAsync();

        if (result != null)
        {
            Songs.Clear();
            foreach (var song in result)
            {
                Songs.Add(song);
            }
        }
    }
}