using AudioHeaven.Services;
using AudioHeaven.Classes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudioHeaven.Models;

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

        [RelayCommand]
        private async Task RemoveQueueSong(Song swipedSong)
        {
            if (swipedSong == null) return;

            int index = musicService.Queue.IndexOf(swipedSong);
            if (index == -1) return;

                musicService.Queue.Remove(swipedSong);
                await API.DeleteQueuePositionAsync(index + 1);
        }
    }
}
