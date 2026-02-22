using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudioHeaven.Pages;
using AudioHeaven.Classes;

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
            if (InputEmail != null && InputPassword != null && InputPassword.Length >= 8)
            {
                AuthResponse? authResponse = await API.LoginAsync(InputEmail, InputPassword);
                await App.Current!.MainPage!.DisplayAlert("Hiba", authResponse.Token, "Ok");

            }
            else
            {
                await App.Current!.MainPage!.DisplayAlert("Hiba", "A nem megfelelő e-mail vagy jelszó!", "Ok");
            }

            //string? id = steamId == null ? InputSteamId : steamId;
            //UserData.PlayerData = await API.GetPlayersData(id);
            //if (UserData.PlayerData != null)
            //{
            //    UserData.GamesInLibrary = await API.GetLibrary(id);
            //    if (UserData.GamesInLibrary == null)
            //    {
            //        await App.Current!.MainPage!.DisplayAlert("Hiba", "Az Ön játékkönyvtárának láthatósága privát! Állítsa át a profil adatait nyilvánosra, majd próbálja újra!", "Ok");
            //        return;
            //    }
            //    await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
            //    return;
            //}
            //await App.Current!.MainPage!.DisplayAlert("Hiba", "A megadott felhasználóval nem érhető el profil vagy a profil láthatósága privát!", "Ok");
        }
    }
}
