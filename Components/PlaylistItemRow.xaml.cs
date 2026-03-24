using AudioHeaven.Models;

namespace AudioHeaven.Components;

public partial class PlaylistItemRow : ContentView
{
	public PlaylistItemRow()
	{
		InitializeComponent();
	}

    private async void OnHeaderClicked(object sender, EventArgs e)
    {
        if (sender is BindableObject bindable && bindable.BindingContext is Playlist p)
        {
            await Shell.Current.GoToAsync($"PlaylistPage?id={p.Id}");
        }
    }
}