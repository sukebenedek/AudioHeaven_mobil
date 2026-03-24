using AudioHeaven.ViewModels;
using CommunityToolkit.Maui.Alerts;

namespace AudioHeaven.Pages;

public partial class LibraryPage : ContentPage
{
    private readonly LibraryViewModel _vm;

    public LibraryPage(LibraryViewModel vm)
	{
		InitializeComponent();
        _vm = vm;
        this.BindingContext = _vm;

    }

    private bool _backPressedOnce = false;

    protected override bool OnBackButtonPressed()
    {
        if (_backPressedOnce) return false; // Let OS handle exit

        _backPressedOnce = true;
        Toast.Make("Press back again to exit").Show();

        Dispatcher.StartTimer(TimeSpan.FromSeconds(2), () => {
            _backPressedOnce = false;
            return false; // Stop timer
        });

        return true;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _vm.LoadAsync();
    }

    private void OnProfileTapped(object sender, EventArgs e)
    {
        LogoutMenu.Open();
    }
}