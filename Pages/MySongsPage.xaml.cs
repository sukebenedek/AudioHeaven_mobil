using AudioHeaven.Classes;
using AudioHeaven.ViewModels;
using CommunityToolkit.Maui.Alerts;

namespace AudioHeaven.Pages;

public partial class MySongsPage : ContentPage
{
    private readonly MySongsViewModel _vm;
    private bool _backPressedOnce = false;

    public MySongsPage(MySongsViewModel vm)
	{
		InitializeComponent();
        _vm = vm;
        BindingContext = _vm;

    }

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

    private bool _isFirstTime = true;

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_isFirstTime)
        {
            _isFirstTime = false; 
            await _vm.LoadAsync();
        }
    }

    private async void OnSongsHeaderClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"AllSongsPage?id={UserData.User.Id}");
        
    }

    private async void OnAlbumsHeaderClicked(object sender, EventArgs e)
    {
        var result = await API.GetUserAlbumsAsync(UserData.User.Id);
        var parameters = new Dictionary<string, object>
        {
            { "albums", result.ToList() } // List<Album>
        };

        await Shell.Current.GoToAsync("SeachedAlbumsPage", parameters);

    }
}