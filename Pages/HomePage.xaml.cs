using AudioHeaven.Classes;
using AudioHeaven.Services;
using AudioHeaven.ViewModels;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
namespace AudioHeaven.Pages;

public partial class HomePage : ContentPage
{


    private bool _backPressedOnce = false;

    protected override bool OnBackButtonPressed()
    {
        if (_backPressedOnce) return false; 

        _backPressedOnce = true;
        Toast.Make("Press back again to exit").Show();

        Dispatcher.StartTimer(TimeSpan.FromSeconds(2), () => {
            _backPressedOnce = false;
            return false; 
        });

        return true;
    }

    private void OnProfileTapped(object sender, EventArgs e)
    {
        LogoutMenu.Open();
    }

    private readonly HomeViewModel _vm;

    private readonly MusicService _musicService;

    public HomePage(HomeViewModel viewModel, MusicService musicService)
    {
        InitializeComponent();
        _vm = viewModel;
        BindingContext = _vm;
        _musicService = musicService;
    }

    private void OnPlayerLoaded(object sender, EventArgs e)
    {
        _musicService.SetPlayer((MediaElement)sender);
    }

    private bool _isFirstTime = true;

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_isFirstTime)
        {
            _isFirstTime = false;
            await _vm.LoadOnceAsync();
        }
        await _vm.LoadAlwaysAsync();

    }

}

