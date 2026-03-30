using AudioHeaven.Classes;
using AudioHeaven.Models;

namespace AudioHeaven.Components;

public partial class LogoutSheetContent : ContentView
{
    public User? User => UserData.User;

    public LogoutSheetContent()
    {
        InitializeComponent();
        lblName.Text = $"Logged in as: {UserData.User?.Name ?? "Guest"}";
        UserData.UserChanged += (_, _) => OnPropertyChanged(nameof(User));
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        if (await API.LogoutAsync())
        {
            await Shell.Current.GoToAsync("//MainPage");
        }
        else
        {
            UserData.DeleteTokenStorage();
            await App.Current!.MainPage!.DisplayAlert("Error", "Unexpexted error occured.", "Ok");
            await Shell.Current.GoToAsync("//MainPage");
        }

        LogoutSheet.IsOpen = false;
    }

    private async void OnProfilePageClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"UserPage?id={UserData.User?.Id}");

        LogoutSheet.IsOpen = false;
    }

    private async void OnProfilePicChangeClicked(object sender, EventArgs e)
    {
        var action = await Shell.Current.DisplayActionSheet(
            "Select Image",
            "Cancel",
            null,
            "Camera",
            "Gallery"
        );

        FileResult photo = null;

        try
        {
            if (action == "Camera")
            {
                photo = await MediaPicker.CapturePhotoAsync();
            }
            else if (action == "Gallery")
            {
                photo = await MediaPicker.PickPhotoAsync();
            }

            if (photo != null)
            {
                var res = await API.UploadProfilePictureAsync(photo);

                if (res != null && res.StatusCode == 200)
                {
                    UserData.User = res.User;
                }
                else
                {
                    throw new Exception("Upload failed");
                }
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }

        Close();
    }

    public void Open()
    {
        LogoutSheet.IsOpen = true;
        lblName.Text = $"Logged in as: {UserData.User?.Name ?? "Guest"}";
    }

    public void Close()
    {
        LogoutSheet.IsOpen = false;
    }
}