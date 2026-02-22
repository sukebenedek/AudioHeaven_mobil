using AudioHeaven.ViewModels;
namespace AudioHeaven.Pages;

public partial class HomePage : ContentPage
{
	public HomePage(HomeViewModel vm)
	{
		InitializeComponent();
        this.BindingContext = vm;
	}
}

