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

            Connectivity.Current.ConnectivityChanged += OnConnectivityChanged;
        }

        protected override async void OnStart()
        {
            base.OnStart();

            // Get the singleton MainViewModel
            var mainVM = Handler.MauiContext.Services.GetService<LoginViewModel>();

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

        private void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess != NetworkAccess.Internet)
            {
                // They just lost internet! 
                // Warning: This runs on a background thread, so you MUST force UI alerts back onto the Main Thread.
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Shell.Current.DisplayAlert("Connection Lost", "You are currently offline.", "OK");
                });
            }
            else
            {
                // Optional: Let them know they are back online
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    // await Shell.Current.DisplayAlert("Back Online", "Your connection has been restored.", "OK");
                });
            }
        }
    }
}
