using AudioHeaven.Models;
using AudioHeaven.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioHeaven.ViewModels
{
    public partial class QueueViewModel : ObservableObject
    {
        [ObservableProperty]
        private MusicService musicService;
        public QueueViewModel(MusicService musicService)
        {
            MusicService = musicService;
        }

        [ObservableProperty]
        private ObservableCollection<Song> songs = new();

        public async Task UpdateSongs()
        {
            await musicService.UpdateQueue();
        }
        

    }
}
