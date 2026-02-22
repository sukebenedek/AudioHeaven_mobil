using AudioHeaven.ViewModels;

namespace AudioHeaven.Pages;

public partial class SearchPage : ContentPage
{
	public SearchPage(SearchViewModel vm)
	{
		InitializeComponent();
		this.BindingContext = vm; 

    }
}