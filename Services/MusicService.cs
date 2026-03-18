using AudioHeaven.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Maui.Views;

namespace AudioHeaven.Services
{
    public partial class MusicService : ObservableObject
    {
        [ObservableProperty]
        private Song currentSong;

        [ObservableProperty]
        private bool isPlaying;

        [ObservableProperty]
        private bool isPlayerVisible = false;

        public MediaElement InternalPlayer { get; set; }

        public void Toggle()
        {
            if (InternalPlayer == null) return;

            if (IsPlaying)
            {
                InternalPlayer.Pause();
            }
            else
            {
                InternalPlayer.Play();
            }
            IsPlaying = !IsPlaying; // This triggers the UI to change the icon
        }

        public void PlaySong(Song song)
        {
            CurrentSong = song;
            IsPlayerVisible = true;

            // Check if the player has been linked yet
            if (InternalPlayer != null)
            {
                InternalPlayer.Source = MediaSource.FromUri(song.FullAudioUrl);
                InternalPlayer.Play();
                IsPlaying = true;
            }
        }
    }
}