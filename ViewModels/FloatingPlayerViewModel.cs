using AudioHeaven.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Views;
using AudioHeaven.Classes;

namespace AudioHeaven.ViewModels
{
    public partial class FloatingPlayerViewModel : ObservableObject
    {
        [ObservableProperty]
        private Song currentSong;

        [ObservableProperty]
        private bool isPlaying;

        public string StreamUrl => $"{API.BaseUrl}/play/{CurrentSong?.Id ?? 1}";


        public FloatingPlayerViewModel()
        {
            // Dummy Data initialization
            CurrentSong = new Song
            {
                Id = 1,
                Title = "Everlong",
                Cover = "storage/covers/KJG4VnftVpFh3izGC0tYVQQSaQ4NLwhh2bXXlyxB.jpg"
            };
        }

        [RelayCommand]
        private void TogglePlay(MediaElement player)
        {
            if (player == null) return;

            if (player.CurrentState == CommunityToolkit.Maui.Core.Primitives.MediaElementState.Playing)
            {
                player.Pause();
                IsPlaying = false;
            }
            else
            {
                player.Play();
                IsPlaying = true;
            }
        }
    }
}