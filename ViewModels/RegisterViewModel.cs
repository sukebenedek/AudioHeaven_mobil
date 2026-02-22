using AudioHeaven.Classes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioHeaven.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        [ObservableProperty]
        string? inputUsername = "GipsyJakab69";

        [ObservableProperty]
        string? inputEmail = "gj@a.com";

        [ObservableProperty]
        string? inputPassword1 = "aaaaaaaa";

        [ObservableProperty]
        string? inputPassword2 = "aaaaaaaa";


        [RelayCommand]
        private async Task RegisterBtnClicked()
        {

            if (string.IsNullOrWhiteSpace(InputEmail) || string.IsNullOrWhiteSpace(InputUsername) ||
                string.IsNullOrWhiteSpace(InputPassword1) || string.IsNullOrWhiteSpace(InputPassword2))
            {
                await App.Current!.MainPage!.DisplayAlert("Error", "All fields are required", "Ok");
                return;
            }
            if (InputPassword1 != InputPassword2)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", "Passwords are not identical", "Ok");
                return;
            }

            try
            {
                AuthResponse? authResponse = await API.RegisterAsync(InputUsername, InputEmail, InputPassword1);

                if (authResponse != null)
                {
                    if (authResponse.Token != null)
                    {
                        //await App.Current!.MainPage!.DisplayAlert("Error", authResponse.Token, "Ok");
                        UserData.User = authResponse.User;
                        UserData.Token = authResponse.Token;

                        await Shell.Current.GoToAsync($"//main/HomePage");
                        return;
                    }
                    else
                    {
                        await App.Current!.MainPage!.DisplayAlert("Error", authResponse.Message, "Ok");
                    }
                }

            }
            catch (Exception)
            {
                await App.Current!.MainPage!.DisplayAlert("Server Error", "The server is currently unavailable.", "Ok");
            }
        }
    }
}
