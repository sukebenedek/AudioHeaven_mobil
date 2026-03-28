using AudioHeaven.Classes;
using AudioHeaven.Models;
using CommunityToolkit.Mvvm.Messaging;

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

    private async void DeletePlaylist(object sender, EventArgs e)
    {
        if (BindingContext is Playlist p)
        {
            bool confirm = await Shell.Current.DisplayAlert(
                "Delete Playlist",
                $"Are you sure you want to delete '{p.Title}'?",
                "Yes, Delete",
                "Cancel");

            if (!confirm) return;

            var success = await API.DeletePlaylistAsync(p.Id);

            if (success)
            {
                WeakReferenceMessenger.Default.Send(new PlaylistDeletedMessage(p.Id));

                await Shell.Current.DisplayAlert("Success", "Playlist deleted successfully.", "OK");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Failed to delete the playlist. Please try again.", "OK");
            }
        }
    }
}