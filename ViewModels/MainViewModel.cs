using AudioHeaven.Classes;
using AudioHeaven.Models;
using AudioHeaven.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioHeaven.ViewModels
{

    public partial class MainViewModel : ObservableObject
    {


        [ObservableProperty]
        string? inputEmail = "suke.benedek@students.jedlik.eu";

        [ObservableProperty]
        string? inputPassword = "aaaaaaaa";

        [ObservableProperty]
        private bool isBusy = false;

        [RelayCommand]
        private async Task DebugBtnClicked()
        {
            await Shell.Current.GoToAsync("//main");
        }

        [RelayCommand]
        private async Task RegisterBtnClicked()
        {
            try
            {
                await Shell.Current.GoToAsync("RegisterPage");
            }
            catch (Exception ex)
            {
                await App.Current!.MainPage!.DisplayAlert("Hiba", ex.Message, "Ok");
            }
        }

        [RelayCommand]
        private async Task LoginBtnClicked()
        {
            if (string.IsNullOrWhiteSpace(InputEmail) || string.IsNullOrWhiteSpace(InputPassword))
            {
                await Shell.Current.DisplayAlert("Error", "All fields are required", "Ok");
                return;
            }

            IsBusy = true;

            try
            {
                AuthResponse? authResponse = await API.LoginAsync(InputEmail, InputPassword);

                if (authResponse?.User != null && authResponse?.Token != null)
                {
                    UserData.User = authResponse.User;
                    UserData.Token = authResponse.Token;
                    await UserData.SaveTokenStorage();


                    await Shell.Current.GoToAsync("//main");
                    return;
                }

                IsBusy = false;
                string msg = authResponse?.Message ?? "Server unavailable.";
                await Shell.Current.DisplayAlert("Error", msg, "Ok");
            }
            catch (Exception)
            {
                IsBusy = false;
                await Shell.Current.DisplayAlert("Error", "Unexpected error occurred.", "Ok");
            }

            IsBusy = false;
        }
    }
}
