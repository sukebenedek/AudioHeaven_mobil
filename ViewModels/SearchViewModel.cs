using AudioHeaven.Classes;
using AudioHeaven.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioHeaven.ViewModels
{
    public partial class SearchViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _searchText;

        [ObservableProperty]
        private ObservableCollection<Album> albums = new();

        [ObservableProperty]
        private ObservableCollection<Song> songs = new();

        [ObservableProperty]
        private ObservableCollection<User> users = new();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsStartVisible))]
        private bool hasAlbums = false;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsStartVisible))]
        private bool hasSongs = false;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsStartVisible))]
        private bool hasUsers = false;

        public bool IsStartVisible => !HasAlbums && !HasSongs && !HasUsers;


        partial void OnSearchTextChanged(string value)
        {
            HasAlbums = Albums.Count() != 0;
            HasSongs = Songs.Count() != 0;
            HasUsers = Users.Count() != 0;

            if (string.IsNullOrWhiteSpace(value))
            {
                Albums.Clear();
                HasAlbums = false;
                Songs.Clear();
                HasSongs = false;
                Users.Clear();
                HasUsers = false;
                return;
            }

            _ = SearchAsync(value);

        }

        private async Task SearchAsync(string query)
        {
            await Task.Delay(300);
            if (query != SearchText) return; 

            UserData.SearchTerm = query;
            var albumResults = await API.GetAlbumsSearchAsync(query, 5);
            var songResults = await API.GetSongsSearchAsync(query, 5);
            var userResults = await API.GetUsersSearchAsync(query, 12);
            HasUsers = userResults.Count() != 0;
            HasAlbums = albumResults.Count() != 0;
            HasSongs = songResults.Count() != 0;

            if (albumResults != null)
            {
                Albums.Clear();
                foreach (var a in albumResults)
                {
                    Albums.Add(a);
                }
            }

            if (songResults != null)
            {
                Songs.Clear();
                foreach (var s in songResults)
                {
                    Songs.Add(s);
                }
            }

            if (userResults != null)
            {
                Users.Clear();
                foreach (var u in userResults)
                {
                    Users.Add(u);
                }
            }
        }
    }
}
