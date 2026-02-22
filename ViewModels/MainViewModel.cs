using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudioHeaven.Pages;
using AudioHeaven.Classes;
using AudioHeaven.Models;

namespace AudioHeaven.ViewModels
{

    public partial class MainViewModel : ObservableObject
    {


        [ObservableProperty]
        string? inputEmail = "a@a.com";

        [ObservableProperty]
        string? inputPassword = "aaaaaaaa";



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
                await App.Current!.MainPage!.DisplayAlert("Error", "All fields are required", "Ok");
                return;
            }

            try
            {
                AuthResponse? authResponse = await API.LoginAsync(InputEmail, InputPassword);

                if (authResponse != null)
                {
                    UserData.User = authResponse.User;
                    UserData.Token = authResponse.Token;

                    await Shell.Current.GoToAsync("//main/HomePage");
                }
                else
                {
                    await App.Current!.MainPage!.DisplayAlert("Error", "Invalid email or password", "Ok");
                }
            }
            catch (Exception)
            {
                await App.Current!.MainPage!.DisplayAlert("Server Error", "The server is currently unavailable.", "Ok");
            }
        }
    }
}
