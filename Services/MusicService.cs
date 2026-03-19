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

        private MediaElement _player;
        private Song _pendingSong;

        // Called once when HomePage loads
        public void SetPlayer(MediaElement player)
        {
            _player = player;

            // If user already selected a song before player was ready
            if (_pendingSong != null)
            {
                PlayInternal(_pendingSong);
                _pendingSong = null;
            }
        }

        public void Toggle()
        {
            if (_player == null) return;

            if (IsPlaying)
            {
                _player.Pause();
                IsPlaying = false;
            }
            else
            {
                _player.Play();
                IsPlaying = true;
            }
        }

        public void PlaySong(Song song)
        {
            CurrentSong = song;
            IsPlayerVisible = true;

            // Player not ready yet → store it
            if (_player == null)
            {
                _pendingSong = song;
                return;
            }

            PlayInternal(song);
        }

        private void PlayInternal(Song song)
        {
            if (_player == null) return;

            _player.Source = MediaSource.FromUri(song.FullAudioUrl);
            _player.Play();
            IsPlaying = true;
        }
    }
}