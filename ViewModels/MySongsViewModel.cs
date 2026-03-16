using AudioHeaven.Classes;
using AudioHeaven.Models;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioHeaven.ViewModels
{
    public partial class MySongsViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Song> songs = new();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(DoesNotHaveSongs))] 
        private bool hasSongs;
        public bool DoesNotHaveSongs => !HasSongs;

        [ObservableProperty]
        private bool isBusy = true;

        public async Task LoadSongsAsync()
        {
            Songs = new ObservableCollection<Song>(await API.GetUserSongsAsync()).OrderByDescending(s => s.Plays).Take(5).ToObservableCollection();
            HasSongs = Songs.Any();
            UserData.Songs = Songs.ToList();
            IsBusy = false;
        }

        [ObservableProperty]
        private ObservableCollection<Album> albums = new();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(DoesNotHaveAlbums))]
        private bool hasAlbums;
        public bool DoesNotHaveAlbums => !HasAlbums;

        //public async Task LoadAlbumsAsync()
        //{
        //    Albums = new ObservableCollection<Album>(await API.GetUserAlbumsAsync()).OrderBy(s => s.CreatedAt).Take(5).ToObservableCollection();
        //    HasAlbums = Albums.Any();
        //    UserData.Albums = Albums.ToList();
        //    IsBusy = false;
        //}
    }
}
