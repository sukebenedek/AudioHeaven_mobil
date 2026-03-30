using AudioHeaven.Classes;
using AudioHeaven.Models;
using AudioHeaven.ViewModels;

namespace AudioHeaven
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            UserAppTheme = AppTheme.Dark;
            MainPage = new AppShell();
        }

        protected override async void OnStart()
        {
            base.OnStart();

            // Get the singleton MainViewModel
            var mainVM = Handler.MauiContext.Services.GetService<MainViewModel>();

            string savedToken = await UserData.GetTokenStorage();

            if (!string.IsNullOrEmpty(savedToken))
            {
                mainVM.IsBusy = true; 
                UserData.Token = savedToken;

                AuthResponse? authResponse = await API.LoginAsyncToken();

                if (authResponse != null && authResponse.User != null && authResponse.User.Id != 0)
                {
                    UserData.User = authResponse.User;
                    await Shell.Current.GoToAsync("//main");
                }
                else
                {
                    UserData.DeleteTokenStorage();
                    mainVM.IsBusy = false; 
                }

            }
        }
    }
}
