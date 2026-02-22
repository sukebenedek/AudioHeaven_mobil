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

        public User user = new User();
        public string token = "";

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
            if (InputEmail != null && InputPassword != null && InputPassword.Length >= 8)
            {
                AuthResponse? authResponse = await API.LoginAsync(InputEmail, InputPassword);

                if (authResponse != null)
                {
                    //await App.Current!.MainPage!.DisplayAlert("Hiba", authResponse.Token, "Ok");
                    user = authResponse.User;
                    token = authResponse.Token;

                    await Shell.Current.GoToAsync($"//main/HomePage");
                    return;
                }
            }
                
            await App.Current!.MainPage!.DisplayAlert("Hiba", "A nem megfelelő e-mail vagy jelszó!", "Ok");
        }
    }
}
