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
}