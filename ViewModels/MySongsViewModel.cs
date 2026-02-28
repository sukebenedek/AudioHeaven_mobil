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
        [NotifyPropertyChangedFor(nameof(DoesNotHaveSongs))] // Tells UI to refresh the "NOT" property too
        private bool hasSongs;
        public bool DoesNotHaveSongs => !HasSongs;

        public async Task LoadSongsAsync()
        {
            Songs = new ObservableCollection<Song>(await API.GetUserSongsAsync()).OrderBy(s => s.Plays).Take(5).ToObservableCollection();
            HasSongs = Songs.Any();
            UserData.Songs = Songs.ToList();
        }
    }
}
