using AudioHeaven.Models;

namespace AudioHeaven.Components;

public partial class AlbumItemRow : ContentView
{
	public AlbumItemRow()
	{
		InitializeComponent();
	}

    private async void OnHeaderClicked(object sender, EventArgs e)
    {
        if (sender is BindableObject bindable && bindable.BindingContext is Album album)
        {
            await Shell.Current.GoToAsync($"AlbumPage?id={album.Id}");
        }
    }

    private async void OnGoToUserPageClicked(object sender, EventArgs e)
    {
        if (BindingContext is Album a && a.User != null)
        {
            //var navParam = new Dictionary<string, User> { { "SelectedUserId", song.User.Id } };
            //await Shell.Current.GoToAsync("UserPage", navParam);
            // Navigation by ID
            await Shell.Current.GoToAsync($"UserPage?id={a.User.Id}");
        }
    }
}