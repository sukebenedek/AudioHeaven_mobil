using AudioHeaven.Classes;
using AudioHeaven.ViewModels;

namespace AudioHeaven
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel vm)
        {
            InitializeComponent();
            this.BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            if (BindingContext is MainViewModel vm)
            {
                vm.IsBusy = false;
            }
        }
    }
}
