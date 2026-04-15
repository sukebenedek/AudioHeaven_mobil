using AudioHeaven.Classes;
using AudioHeaven.Models;
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
        string? inputUsername = "SukeBenedek";

        [ObservableProperty]
        string? inputEmail = "suke.benedek@students.jedlik.eu";

        [ObservableProperty]
        string? inputPassword1 = "aaaaaaaa";

        [ObservableProperty]
        string? inputPassword2 = "aaaaaaaa";

        [ObservableProperty]
        private bool isBusy = false;

        [RelayCommand]
        private async Task RegisterBtnClicked()
        {
            if (string.IsNullOrWhiteSpace(InputEmail) || string.IsNullOrWhiteSpace(InputUsername) ||
                string.IsNullOrWhiteSpace(InputPassword1) || string.IsNullOrWhiteSpace(InputPassword2))
            {
                await Shell.Current.DisplayAlert("Error", "All fields are required", "Ok");
                return;
            }

            if (InputPassword1 != InputPassword2)
            {
                await Shell.Current.DisplayAlert("Error", "Passwords are not identical", "Ok");
                return;
            }

            IsBusy = true;

            try
            {
                AuthResponse? authResponse = await API.RegisterAsync(InputUsername, InputEmail, InputPassword1);

                if (authResponse?.Token != null && authResponse?.User != null)
                {
                    UserData.User = authResponse.User;
                    UserData.Token = authResponse.Token;
                    await UserData.SaveTokenStorage();

                    await Shell.Current.GoToAsync("//main");
                    return;
                }

                IsBusy = false;
                string msg = authResponse?.Message ?? "Registration failed.";
                await Shell.Current.DisplayAlert("Error", msg, "Ok");
            }
            catch (Exception)
            {
                IsBusy = false;
                await Shell.Current.DisplayAlert("Server Error", "The server is currently unavailable.", "Ok");
            }

            IsBusy = false;
        }
    }
}
