using AudioHeaven.Classes;
using AudioHeaven.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioHeaven.ViewModels
{
    public partial class LibraryViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Playlist> playlists = new();

        [ObservableProperty]
        private bool hasPlaylists;

        [ObservableProperty]
        private bool isBusy = true;

        public async Task LoadAsync()
        {
            Playlists = new ObservableCollection<Playlist>(await API.GetUserPlaylistsAsync());
            HasPlaylists = Playlists.Any();
            IsBusy = false;
        }
        public LibraryViewModel()
        {
            WeakReferenceMessenger.Default.Register<PlaylistDeletedMessage>(this, async (recipient, message) =>
            {
                await LoadAsync();
            });
        }
        [RelayCommand]
        private async Task CreatePlaylist()
        {
            string playlistName = await Shell.Current.DisplayPromptAsync(
                "New Playlist",
                "Enter a name for your playlist:",
                "Create",
                "Cancel",
                "My Awesome Mix");

            if (!string.IsNullOrWhiteSpace(playlistName))
            {
                var newPlaylist = await API.CreatePlaylistAsync(playlistName.Trim());

                if (newPlaylist != null)
                {
                    await LoadAsync();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Could not create playlist. Please try again.", "OK");
                }
            }
        }
    }
}
