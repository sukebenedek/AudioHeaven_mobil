using AudioHeaven.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Views;
using AudioHeaven.Classes;
using AudioHeaven.Services;

namespace AudioHeaven.ViewModels
{
    public partial class FloatingPlayerViewModel : ObservableObject
    {
        [ObservableProperty]
        private MusicService musicService;

        public string StreamUrl => $"{API.BaseUrl}/play/1";

        public FloatingPlayerViewModel(MusicService musicService)
        {
            MusicService = musicService;

            // Initialize dummy data in the service if empty
            if (MusicService.CurrentSong == null)
            {
                MusicService.CurrentSong = new Song { Id = 1, Title = "Song Title" , Cover= $"storage/covers/KJG4VnftVpFh3izGC0tYVQQSaQ4NLwhh2bXXlyxB.jpg" };
            }
        }

        [RelayCommand]
        private void TogglePlay(MediaElement player)
        {
            // Link the UI player to the service instance
            if (MusicService.InternalPlayer == null)
                MusicService.InternalPlayer = player;

            MusicService.Toggle();
        }
    }
}