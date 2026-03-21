using AudioHeaven.Classes;
using AudioHeaven.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace AudioHeaven.Pages;

[QueryProperty(nameof(UserIdString), "id")]
public partial class AllSongsPage : ContentPage
{
    public ObservableCollection<Song> Songs { get; set; } = new();

    private int? _userId;

    public string UserIdString
    {
        set
        {
            if (int.TryParse(value, out int id))
                _userId = id;
        }
    }

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
        Title = $"{UserData.User.Name}'s Songs";
        IsBusy = true;

        try
        {
            int id = _userId ?? UserData.User.Id;

            var result = await API.GetUserSongsAsync(id);

            if (result != null)
            {
                Songs.Clear();
                foreach (var song in result)
                    Songs.Add(song);
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

}