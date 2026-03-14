using AudioHeaven.ViewModels;
using CommunityToolkit.Maui.Alerts;

namespace AudioHeaven.Pages;

public partial class SearchPage : ContentPage
{
	public SearchPage(SearchViewModel vm)
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

    private async void OnAlbumsHeaderClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("SeachedAlbumsPage");
    }
}