using AudioHeaven.ViewModels;
using CommunityToolkit.Maui.Alerts;

namespace AudioHeaven.Pages;

public partial class MySongsPage : ContentPage
{
	public MySongsPage(MySongsViewModel vm)
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

}