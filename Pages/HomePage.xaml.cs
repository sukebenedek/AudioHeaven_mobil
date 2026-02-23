using AudioHeaven.Classes;
using AudioHeaven.ViewModels;
using CommunityToolkit.Maui.Alerts;
namespace AudioHeaven.Pages;

public partial class HomePage : ContentPage
{
	public HomePage(HomeViewModel vm)
	{
		InitializeComponent();
        this.BindingContext = vm;
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

    private void OnProfileTapped(object sender, EventArgs e)
    {
        LogoutSheet.IsOpen = true;
    }

    private async void OnLogoutRequestedOnPage(object sender, EventArgs e)
    {
        LogoutSheet.IsOpen = false; 

        UserData.User = null;
        UserData.Token = null;

        await Shell.Current.GoToAsync("//MainPage");
    }
}

