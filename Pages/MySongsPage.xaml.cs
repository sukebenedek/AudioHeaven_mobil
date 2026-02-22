using AudioHeaven.ViewModels;

namespace AudioHeaven.Pages;

public partial class MySongsPage : ContentPage
{
	public MySongsPage(MySongsViewModel vm)
	{
		InitializeComponent();
        this.BindingContext = vm;

    }
}