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

        await Task.Delay(3);

        try
        {
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
        finally
        {
            IsBusy = false;
        }
    }

}