using AudioHeaven.Classes;
using AudioHeaven.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using IntelliJ.Lang.Annotations;
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
    }
}
