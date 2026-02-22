using AudioHeaven.ViewModels;

namespace AudioHeaven.Pages;

public partial class LibraryPage : ContentPage
{
	public LibraryPage(LibraryViewModel vm)
	{
		InitializeComponent();
        this.BindingContext = vm;

    }
}