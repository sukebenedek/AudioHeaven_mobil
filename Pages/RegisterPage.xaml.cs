namespace AudioHeaven.Pages;
using AudioHeaven.Pages;
using AudioHeaven.ViewModels;

public partial class RegisterPage : ContentPage
{
	public RegisterPage(RegisterViewModel vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        if (BindingContext is RegisterViewModel vm)
        {
            vm.IsBusy = false;
        }
    }
}