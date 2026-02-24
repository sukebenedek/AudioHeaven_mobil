using AudioHeaven.Classes;

namespace AudioHeaven.Components;

public partial class LogoutSheetContent : ContentView
{
    public event EventHandler LogoutRequested;

    public LogoutSheetContent()
    {
        InitializeComponent();
        lblName.Text = $"Logged in as: {UserData.User?.Name ?? "Guest"}";
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        if (await API.LogoutAsync())
        {
            UserData.User = null;
            UserData.Token = null;
            await Shell.Current.GoToAsync("//MainPage");
        }
        else
        {
            await App.Current!.MainPage!.DisplayAlert("Error", "Unexpexted error occured.", "Ok");
        }

        LogoutSheet.IsOpen = false;
    }

    public void Open()
    {
        LogoutSheet.IsOpen = true;
    }

    public void Close()
    {
        LogoutSheet.IsOpen = false;
    }
}