using AudioHeaven.Classes;
using AudioHeaven.ViewModels;

namespace AudioHeaven
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage(LoginViewModel vm)
        {
            InitializeComponent();
            this.BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            if (BindingContext is LoginViewModel vm)
            {
                vm.IsBusy = false;
            }
        }
    }
}
